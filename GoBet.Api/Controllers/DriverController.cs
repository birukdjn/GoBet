using GoBet.Api.Extensions;
using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
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
            string UserId = User.GetUserId().ToString();
            await driverService.RequestDriverAsync(UserId, dto.LicenseNumber);
            return Ok(new { message = "Driver request submitted" });
        }

    }
}