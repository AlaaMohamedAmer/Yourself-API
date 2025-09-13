using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.DTOs;
using yourself_demoAPI.Repository.Dashboard;

namespace yourself_demoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepo _dashboardRepo;
        public DashboardController(IDashboardRepo dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        [HttpPost("add-system-user")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddSystemUser(SystemUserRegisterDTO request)
        {
            var response = await _dashboardRepo.AddSystemUser(request);
            if (response is null)
                return BadRequest("Email already in use");
            return Ok(response);
        }
    }
}
