using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using yourself_demoAPI.Data;
using yourself_demoAPI.DTOs.Profile;

namespace Yourself_App.Repository.Profile
{
	public class ProfileService : IProfileService
	{
		private readonly ApplicationDBContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProfileService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<object?> GetProfileAsync(Guid userId)
		{
			var user = await _context.Users
							.Include(u => u.School)
							.FirstOrDefaultAsync(u => u.Account.Id == userId)
							?? throw new Exception("User Not Found");

			if (user == null) return null;

			return new ProfileDTO
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Phone = user.PhoneNumber,
				SchoolName = user.School?.Name,
				ProfilePictureUrl = user.ProfilePictureUrl
			};
		}

		public async Task UpdateProfileAsync(Guid userId, UpdateProfileDTO updateProfileDTO)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Account.Id == userId)
							?? throw new Exception("User Not Found");

			if (!string.IsNullOrWhiteSpace(updateProfileDTO.FirstName))
				user.FirstName = updateProfileDTO.FirstName;

			if (!string.IsNullOrWhiteSpace(updateProfileDTO.LastName))
				user.LastName = updateProfileDTO.LastName;

			if (!string.IsNullOrWhiteSpace(updateProfileDTO.Email))
				user.Email = updateProfileDTO.Email;

			if (!string.IsNullOrWhiteSpace(updateProfileDTO.Phone))
				user.PhoneNumber = updateProfileDTO.Phone;

			await _context.SaveChangesAsync();
		}

		public async Task<UploadPhotoResponseDTO> UploadProfilePictureAsync(Guid userId, IFormFile file)
		{
			if (file == null || file.Length == 0)
				throw new Exception("No File Uploaded");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.AccountId == userId)
							?? throw new Exception("User Not Found");

			var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
			if(!Directory.Exists(uploadsPath))
				Directory.CreateDirectory(uploadsPath);

			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			var filePath = Path.Combine(uploadsPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			user.ProfilePictureUrl = $"/uploads/{fileName}";

			var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
			user.ProfilePictureUrl = $"{baseUrl}/uploads/{fileName}";
			await _context.SaveChangesAsync();

			return new UploadPhotoResponseDTO
			{
				ProfilePictureUrl = user.ProfilePictureUrl,
			};
		}
	}
}