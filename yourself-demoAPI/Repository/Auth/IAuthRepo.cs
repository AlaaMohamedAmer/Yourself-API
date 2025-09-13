using yourself_demoAPI.DTOs.Auth;
using yourself_demoAPI.Models.Auth;

namespace yourself_demoAPI.Repository.Auth
{
    public interface IAuthRepo
    {
		Task RegisterAsync(UserRegisterDTO request);
		Task RequestResetAsync(string email);
		Task ResetPasswordAsync(string email, string code, string newPassword);
		Task<TokenResponeDTO?> LoginAsync(LoginDTO request);
        Task<TokenResponeDTO?> CreateNewTokens(RefreshTokenRequestDTO request);
    }
}
