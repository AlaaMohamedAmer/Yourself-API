using System.Text.Json.Serialization;

namespace yourself_demoAPI.Models.Auth
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [JsonIgnore]
        public Admin? Admin { get; set; }
		public User? User { get; set; }

	}
}
