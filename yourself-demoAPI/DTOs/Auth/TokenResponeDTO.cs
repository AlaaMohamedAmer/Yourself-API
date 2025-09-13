namespace yourself_demoAPI.DTOs.Auth
{
    public class TokenResponeDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
