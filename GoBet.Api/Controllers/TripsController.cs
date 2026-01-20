<<<<<<< HEAD
﻿using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4

namespace GoBet.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
<<<<<<< HEAD
    [Authorize(Roles = Roles.Passenger)]
    public class TripsController : ControllerBase
    {
=======
    public class TripsController : ControllerBase
    {
        // We inject the service/repository via the constructor (Dependency Injection)
        //private readonly ITripService _tripService;
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4

        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookSeat(Guid id)
        {
<<<<<<< HEAD
=======
            //await _tripService.ProcessBooking(id);
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
            return Ok("Seat booked successfully.");
        }
    }
}