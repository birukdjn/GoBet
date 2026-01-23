using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GoBet.Application.DTOs;
using GoBet.Application.Interfaces;


namespace GoBet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Passenger")]
public class PassengerController(IPassengerService passengerService) : ControllerBase
{
    [HttpGet("find-buses")]
    public async Task<IActionResult> GetNearbyBuses([FromQuery] double lat, [FromQuery] double lon, [FromQuery] string destination)
    {
        var buses = await passengerService.FindNearbyBusesAsync(lat, lon, destination);
        return Ok(buses);
    }

    [HttpPost("book-pickup")]
    public async Task<IActionResult> BookPickup([FromBody] BookingRequest request)
    {
        // Automatically grab Passenger ID from the JWT Token
        request.PassengerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var result = await passengerService.BookRoadsidePickupAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new {ex.Message });
        }
    }

    [HttpGet("nearest-terminal-buses")]
    public async Task<IActionResult> GetBusesAtTerminal([FromQuery] double lat, [FromQuery] double lon, [FromQuery] string destination)
    {
        try
        {
            var result = await passengerService.FindBusesAtNearestTerminalAsync(lat, lon, destination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new {ex.Message });
        }
    }
}