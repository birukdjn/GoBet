using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
 
            await authService.ForgotPasswordAsync(model.Email);

            return Ok(new { Message = "If an account exists with this email, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await authService.ResetPasswordAsync(model);
                return Ok(new { Message = "Password has been reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
