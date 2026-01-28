using GoBet.Api.Extensions;
using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController(ITripService tripService) : ControllerBase
    {
        private readonly ITripService _tripService = tripService;

        [HttpPost]
        [Authorize(Roles = Roles.Driver)]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
        {
            var driverId = User.GetUserId();
            var tripId = await _tripService.CreateTripAsync(request, driverId);

            return CreatedAtAction(nameof(GetTrip), new { id = tripId }, null);
        }


        // -------------------------
        // Get trip by id
        // -------------------------
        [HttpGet("{id}")]
        [Authorize(Roles = "Passenger,Driver")]
        public async Task<IActionResult> GetTrip(Guid id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null) return NotFound();
            return Ok(new TripDto(trip));
        }     

        // -------------------------
        // Start trip (Driver)
        // -------------------------
        [HttpPost("{id}/start")]
        [Authorize(Roles = Roles.Driver)]
        public async Task<IActionResult> StartTrip(Guid id)
        {
            var driverId = User.GetUserId();
            await _tripService.StartTripAsync(id, driverId);
            return Ok(new { Message = "Trip started successfully." });
        }

        // -------------------------
        // Complete trip (Driver)
        // -------------------------
        [HttpPost("{id}/complete")]
        [Authorize(Roles = Roles.Driver)]
        public async Task<IActionResult> CompleteTrip(Guid id)
        {
            var driverId = User.GetUserId();
            await _tripService.CompleteTripAsync(id, driverId);
            return Ok(new { Message = "Trip completed successfully." });
        }

        // -------------------------
        // Update location (Driver)
        // -------------------------
        [HttpPut("{id}/location")]
        [Authorize(Roles = Roles.Driver)]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationUpdateRequest request)
        {
            var driverId = User.GetUserId();
            await _tripService.UpdateLocationAsync(
                id, 
                driverId, 
                request.Latitude, 
                request.Longitude);

            return Ok(new { Message = "Location updated successfully." });
        }
    }
}
