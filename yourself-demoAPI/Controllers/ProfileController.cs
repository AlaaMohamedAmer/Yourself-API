using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yourself_App.Repository.Profile;
using yourself_demoAPI.DTOs.Profile;

namespace Yourself_App.Controllers.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly IProfileService _profileService;
		public ProfileController(IProfileService profileService)
		{
			_profileService = profileService;
		}

		[HttpGet("Get-Profile")]
		public async Task<IActionResult> GetProfile()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();
			var profile = await _profileService.GetProfileAsync(Guid.Parse(userId));

			return Ok(profile);
		}

		[HttpPut("Update-Profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();
			await _profileService.UpdateProfileAsync(Guid.Parse(userId), updateProfileDTO);

			return NoContent();
		}
		   
		[HttpPost("Upload-Photo")]
		public async Task<IActionResult> UploadPhoto(IFormFile formFile)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();
			
			var result =await _profileService.UploadProfilePictureAsync(Guid.Parse(userId), formFile);
			return Ok (result);
		}
	}
}
