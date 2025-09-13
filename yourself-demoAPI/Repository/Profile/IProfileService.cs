using yourself_demoAPI.DTOs.Profile;

namespace Yourself_App.Repository.Profile
{
	public interface IProfileService
	{
		Task<object?> GetProfileAsync(Guid userId);
		Task UpdateProfileAsync(Guid userId, UpdateProfileDTO updateProfileDTO);
		Task<UploadPhotoResponseDTO> UploadProfilePictureAsync(Guid userId, IFormFile file);

	}
}
