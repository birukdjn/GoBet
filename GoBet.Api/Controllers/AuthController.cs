using Microsoft.AspNetCore.Mvc;
using GoBet.Application.Services.Interfaces;
using GoBet.Application.DTOs;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _userService.RegisterPassengerAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully!" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("request-driver-role")]
        public async Task<IActionResult> RequestDriver([FromBody] DriverRequestDto request)
        {
            await _userService.RequestDriverStatusAsync(request.UserId, request.LicenseNumber);
            return Ok(new { message = "Driver request submitted for approval." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _userService.LoginAsync(model);

            if (token != null)
            {
                return Ok(new { Token = token, Message = "Login Successful" });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
