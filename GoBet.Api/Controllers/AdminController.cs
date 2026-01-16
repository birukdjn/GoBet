using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoBet.Domain.Constants;
using GoBet.Application.Services.Interfaces;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("approve-driver/{userId}")]
        public async Task<IActionResult> ApproveDriver(string userId)
        {
            await _userService.ApproveDriverAsync(userId);
            return Ok(new { message = "User has been approved as a driver." });
        }
    }
}
