using GoBet.Application.DTOs;
using GoBet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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