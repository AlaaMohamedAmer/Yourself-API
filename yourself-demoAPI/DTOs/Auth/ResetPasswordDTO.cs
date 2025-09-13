namespace yourself_demoAPI.DTOs.Auth
{
	public class ResetPasswordDTO
	{
		public string? Email { get; set; }
		public string? Code { get; set; }
		public string? Password { get; set; }
		public string? ConfirmPassword { get; set; }
	}
}
