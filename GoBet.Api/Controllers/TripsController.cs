using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GoBet.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Passenger)]
    public class TripsController : ControllerBase
    {
        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookSeat(Guid id)
        {
            return Ok("Seat booked successfully.");
        }
    }
}