using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public IActionResult LoginGoogle([FromQuery] string? redirectUrl)
        {
            var callbackUrl = Url.Action(nameof(ExternalLoginCallback));

            var properties = new AuthenticationProperties { RedirectUri = callbackUrl };

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                properties.Items["nextjs_redirect"] = redirectUrl;
            }
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

          }

        [HttpGet("login-facebook")]
        public IActionResult LoginFacebook([FromQuery] string? redirectUrl)
        {
            var callbackUrl = Url.Action(nameof(ExternalLoginCallback));

            var properties = new AuthenticationProperties { RedirectUri = callbackUrl };

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                properties.Items["nextjs_redirect"] = redirectUrl;
            }

            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await authService.HandleExternalLoginAsync(HttpContext);

            var authProps = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            var targetUrl = authProps?.Properties?.Items.ContainsKey("nextjs_redirect") == true
                            ? authProps.Properties.Items["nextjs_redirect"]
                            : "http://localhost:3000/auth-callback";

            var finalUrl = $"{targetUrl}?token={Uri.EscapeDataString(result.Token)}";

            return Redirect(finalUrl);

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
