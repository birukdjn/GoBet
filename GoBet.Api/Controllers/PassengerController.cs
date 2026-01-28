using GoBet.Api.Extensions;
using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/passenger")]
    public class PassengerController(IPassengerService passengerService) : ControllerBase
    {
        [HttpGet("find-buses")]
        public async Task<IActionResult> FindNearby(double lat, double lon, string destination)
            => Ok(await passengerService.FindNearbyBusesAsync(lat, lon, destination));

        [HttpGet("nearest-terminal-buses")]
        public async Task<IActionResult> NearestTerminal(double lat, double lon, string destination)
            => Ok(await passengerService.FindBusesAtNearestTerminalAsync(lat, lon, destination));

        [Authorize(Roles = "Passenger")]
        [HttpPost("book-pickup")]
        public async Task<IActionResult> BookPickup(BookingRequest request)
        {
            string passengerId = User.GetUserId().ToString();
            return Ok(await passengerService.BookRoadsidePickupAsync(
                request.TripId, 
                passengerId, 
                request.Latitude, 
                request.Longitude));
        }
    }
}