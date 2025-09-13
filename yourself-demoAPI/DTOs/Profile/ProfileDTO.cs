namespace yourself_demoAPI.DTOs.Profile
{
	public class ProfileDTO
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty ;
		public string Phone { get; set; } = string.Empty;
		public string? SchoolName { get; set; } 
		public string? ProfilePictureUrl { get; set; }
	}
}
