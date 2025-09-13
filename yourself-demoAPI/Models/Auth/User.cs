using Yourself_App.Models.School;

namespace yourself_demoAPI.Models.Auth
{
	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = "";
		public string PhoneNumber { get; set; } = string.Empty;
		public string? ProfilePictureUrl { get; set; }
		public string PasswordHash { get; set; } = "";

		public string? ResetHash { get; set; }
		public DateTime? ResetExpiry { get; set; }

		public Guid AccountId { get; set; }
		public Account Account { get; set; } = null!;

		public Guid SchoolId { get; set; }
		public School School { get; set; } = null!;
	}
}
