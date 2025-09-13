using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Yourself_App.Models.School;
using yourself_demoAPI.Data;
using yourself_demoAPI.Data.Helpers;
using yourself_demoAPI.DTOs.Auth;
using yourself_demoAPI.Models.Auth;
using yourself_demoAPI.Repository.EmailSender;
using static yourself_demoAPI.Data.Helpers.Data;

namespace yourself_demoAPI.Repository.Auth
{
	public class AuthRepo : IAuthRepo
	{
		private readonly ApplicationDBContext _context;
		private readonly IConfiguration _config;
		public readonly ISmtpEmailSender _emailSender;
		public AuthRepo(ApplicationDBContext context, IConfiguration config, ISmtpEmailSender emailSender)
		{
			_context = context;
			_config = config;
			_emailSender = emailSender;
		}


		public async Task RegisterAsync(UserRegisterDTO request)
		{
			if (await _context.Users.AnyAsync(a => a.Email == request.Email))
				throw new Exception("Email exists");

			var school = await _context.Schools.FirstOrDefaultAsync(s => s.Code == request.SchoolCode)
			?? throw new Exception("School not found");

			var account = new Account
			{
				Id = Guid.NewGuid(),
				Email = request.Email,
				Role = "User",
			};

			var hasher = new PasswordHasher<Account>();
			account.HashedPassword = hasher.HashPassword(account, request.Password);

			var user = new User
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber,
				PasswordHash = account.HashedPassword,
				SchoolId = school.Id,
				Account = account
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

		}

		public async Task RequestResetAsync(string email)
		{
			var user = await _context.Users.Include(u => u.Account)
							.FirstOrDefaultAsync(u => u.Email == email)
			  ?? throw new Exception("Not found");

			var otp = OtpHelper.GenerateCode();
			user.ResetHash = OtpHelper.HashCode(otp, _config["Secrets:Otp"]);
			user.ResetExpiry = DateTime.UtcNow.AddMinutes(10);

			await _context.SaveChangesAsync();
			await _emailSender.SendAsync(email, "Password reset code", $"Your reset code is {otp}");
		}

		public async Task ResetPasswordAsync(string email, string code, string newPassword)
		{
			var user = await _context.Users.Include(u => u.Account)
							.FirstOrDefaultAsync(u => u.Email == email)
			 ?? throw new Exception("Not Found");

			if (user.ResetExpiry < DateTime.UtcNow)
				throw new Exception("Expired");

			if (!OtpHelper.VerifyCode(code, user.ResetHash!, _config["Secrets:Otp"]))
				throw new Exception("Invalid code");

			var hasher = new PasswordHasher<Account>();
			user.Account.HashedPassword = hasher.HashPassword(user.Account, newPassword);

			user.ResetHash = null;
			user.ResetExpiry = null;

			await _context.SaveChangesAsync();
		}
		public async Task<TokenResponeDTO?> LoginAsync(LoginDTO request)
		{
			var account = _context.Accounts.FirstOrDefault(a => a.Email == request.Email);

			if (account is null || new PasswordHasher<Account>().VerifyHashedPassword(account, account.HashedPassword, request.Password)
				== PasswordVerificationResult.Failed)
				return null;

			var response = new TokenResponeDTO
			{
				AccessToken = CreateAccessToken(account),
				RefreshToken = await CreateRefreshToken(account)
			};

			return response;
		}

		private string CreateAccessToken(Account account)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
				new Claim(ClaimTypes.Email, account.Email),
				new Claim(ClaimTypes.Role, account.Role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("AppSettings:Token")!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var tokenDescriptor = new JwtSecurityToken(
				issuer: _config.GetValue<string>("AppSettings:Issuer"),
				audience: _config.GetValue<string>("AppSettings:Audience"),
				claims: claims,
				expires: DateTime.Now.AddMinutes(_config.GetValue<Double>("AppSettings:DurationInMinutes")),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
		}

		private async Task<string> CreateRefreshToken(Account account)
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			var refreshToken = Convert.ToBase64String(randomNumber);

			account.RefreshToken = refreshToken;
			account.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

			await _context.SaveChangesAsync();
			return refreshToken;
		}

		private async Task<Account?> ValidateRefreshTokenAsync(RefreshTokenRequestDTO request)
		{
			var account = await _context.Accounts.FindAsync(request.AccountId);

			if (account is null || account.RefreshToken != request.RefreshToken
				|| account.RefreshTokenExpiryTime <= DateTime.UtcNow)
				return null;

			return account;
		}

		public async Task<TokenResponeDTO?> CreateNewTokens(RefreshTokenRequestDTO request)
		{
			var account = await ValidateRefreshTokenAsync(request);

			if (account is null)
				return null;

			return new TokenResponeDTO
			{
				AccessToken = CreateAccessToken(account),
				RefreshToken = await CreateRefreshToken(account)
			};
		}
	}
}