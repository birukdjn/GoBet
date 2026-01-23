using GoBet.Application.DTOs;
using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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


        [HttpPost("start-trip")]
        //[Authorize(Roles = Roles.Driver)]
        public async Task<IActionResult> StartTrip([FromBody] StartTripRequestDto dto)
        {
            // Get Driver ID from the logged-in user token
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var tripId = await driverService.StartTripAsync(driverId!, dto.Destination, dto.TerminalIds);
                return Ok(new { TripId = tripId, Message = "Trip started successfully. Passengers can now find you." });
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message });
            }
        }

    }
}