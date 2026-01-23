
using GoBet.Application.Services;
using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController(AdminService adminService) : ControllerBase
    {
        [HttpPost("approve-driver/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApproveDriver(string userId)
        {
            await adminService.ApproveDriverAsync(userId);

            return Ok(new { message = "User has been approved as a driver." });

        }
    }
}
