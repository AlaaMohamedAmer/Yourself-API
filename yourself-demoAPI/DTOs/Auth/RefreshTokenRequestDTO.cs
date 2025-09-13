namespace yourself_demoAPI.DTOs.Auth
{
    public class RefreshTokenRequestDTO
    {
        public Guid AccountId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
