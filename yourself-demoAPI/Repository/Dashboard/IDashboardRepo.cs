using yourself_demoAPI.DTOs;
using yourself_demoAPI.Models.Auth;

namespace yourself_demoAPI.Repository.Dashboard
{
    public interface IDashboardRepo
    {
        Task<Account?> AddSystemUser(SystemUserRegisterDTO request);
    }
}
