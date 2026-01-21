using GoBet.Application.DTOs;
using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController(IDriverService driverService) : ControllerBase
    {
        [Authorize(Roles = Roles.Passenger)]
        [HttpPost("request-driver")]
        public async Task<IActionResult> RequestDriver(DriverRequestDto dto)
        {
            await driverService.RequestDriverAsync(dto.UserId, dto.LicenseNumber);
            return Ok(new { message = "Driver request submitted" });
        }

        [HttpPost("approve-driver/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApproveDriver(string userId)
        {
            await driverService.ApproveDriverAsync(userId);

            return Ok(new { message = "User has been approved as a driver." });

        }

    }
}