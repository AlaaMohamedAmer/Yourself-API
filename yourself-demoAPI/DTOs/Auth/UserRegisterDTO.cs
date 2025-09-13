using System.ComponentModel.DataAnnotations;
using static yourself_demoAPI.Data.Helpers.Data;

namespace yourself_demoAPI.DTOs.Auth
{
    public class UserRegisterDTO
    {
        public string? SchoolCode { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
        [MinLength(8)] 
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be strong.")]
        public string Password { get; set; } = string.Empty;

		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string? ConfirmPassword { get; set; }
	}
}
