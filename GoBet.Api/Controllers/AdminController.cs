
using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
  
        [HttpGet("driver-requests")]
        public async Task<IActionResult> GetDriverRequests()
        {
            var requests = await adminService.GetPendingDriverRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("approve-driver/{userId}")]
        public async Task<IActionResult> Approve(string userId)
        {
            await adminService.ApproveDriverAsync(userId);
            return Ok(new { message = "Driver approved successfully" });
        }

        [HttpPost("reject-driver/{userId}")]
        public async Task<IActionResult> Reject(string userId, [FromBody] string reason)
        {
            await adminService.RejectDriverAsync(userId, reason);
            return Ok(new { message = "Driver request rejected" });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await adminService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        [HttpPatch("users/{userId}/role")]
        public async Task<IActionResult> UpdateRole(string userId, [FromBody] RoleUpdateDto dto)
        {
            await adminService.ChangeUserRoleAsync(userId, dto.Role);
            return Ok(new { message = $"Role updated to {dto.Role}" });
        }

        [HttpPatch("users/{userId}/status")]
        public async Task<IActionResult> ToggleStatus(string userId)
        {
            await adminService.UpdateUserStatusAsync(userId);
            return Ok(new { message = "User status toggled successfully." });
        }
    }
}
