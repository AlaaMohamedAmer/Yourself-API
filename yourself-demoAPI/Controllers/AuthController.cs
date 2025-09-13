using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.DTOs.Auth;
using yourself_demoAPI.Repository.Auth;

namespace yourself_demoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

		//to do: add-admin should only be for the super admin
		//to do: validate the input [email-password]

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO request)
		{
			try
			{
				await _authRepo.RegisterAsync(request);
				return Ok(new { Message = "Registration successful" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { ex.Message });
			}
		}


		[HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var response = await _authRepo.LoginAsync(request);

            if (response is null)
                return BadRequest("Invalid credentials");

            return Ok(response);
        }

        [HttpPost("new-tokens")]
        public async Task<IActionResult> GetNewTokens(RefreshTokenRequestDTO request)
        {
            var respone = await _authRepo.CreateNewTokens(request);

            if (respone is null || respone.AccessToken is null || respone.RefreshToken is null)
                return Unauthorized("Invalid Tokens");

            return Ok(respone);
        }

		[HttpPost("request-reset")]
		public async Task<IActionResult> RequestReset([FromBody] RequestResetDTO requestResetDTO)
		{
			try
			{
				await _authRepo.RequestResetAsync(requestResetDTO.Email);
				return Ok(new { Message = "Reset code sent to your email." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { ex.Message });
			}
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
		{
			try
			{
				await _authRepo.ResetPasswordAsync(resetPasswordDTO.Email, resetPasswordDTO.Code, resetPasswordDTO.Password);
				return Ok(new { Message = "Password reset successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { ex.Message });
			}
		}
	}
}
