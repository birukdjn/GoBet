using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        // We inject the service/repository via the constructor (Dependency Injection)
        //private readonly ITripService _tripService;

        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookSeat(Guid id)
        {
            //await _tripService.ProcessBooking(id);
            return Ok("Seat booked successfully.");
        }
    }
}