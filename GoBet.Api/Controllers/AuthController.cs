<<<<<<< HEAD
﻿using GoBet.Application.DTOs;
using GoBet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
using GoBet.Application.Services.Interfaces;
using GoBet.Application.DTOs;
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
<<<<<<< HEAD
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await authService.RegisterPassengerAsync(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await authService.LoginAsync(model);
            return Ok(result);
        }
      

        [HttpGet("login-google")]
        public IActionResult LoginGoogle() =>
            Challenge(new AuthenticationProperties { RedirectUri = Url.Action(nameof(ExternalLoginCallback)) }, GoogleDefaults.AuthenticationScheme);

        [HttpGet("login-facebook")]
        public IActionResult LoginFacebook() =>
            Challenge(new AuthenticationProperties { RedirectUri = Url.Action(nameof(ExternalLoginCallback)) }, FacebookDefaults.AuthenticationScheme);

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await authService.HandleExternalLoginAsync(HttpContext);
            return Ok(result);
        }
    }
}
=======
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
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
