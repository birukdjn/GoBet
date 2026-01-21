using GoBet.Application.DTOs;
using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using GoBet.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GoBet.Infrastructure.Identity
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ITokenService tokenService)
        : IAuthService
    {
        public async Task RegisterPassengerAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, Roles.Passenger);
        }

        public async Task<AuthResultDto> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email)
                ?? throw new Exception("Invalid credentials");

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception("Invalid credentials");

            var token = await tokenService.GenerateAccessTokenAsync(user);

            return new AuthResultDto(token, DateTime.UtcNow.AddHours(12));
        }

        public async Task<AuthResultDto> HandleExternalLoginAsync(HttpContext context)
        {
            var result = await context.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded || result.Principal == null)
                throw new Exception("External authentication failed");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email)
                ?? throw new Exception("Email not provided by provider");

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FullName = result.Principal.FindFirstValue(ClaimTypes.Name) ?? email,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    throw new Exception("Failed to create user from social login");

                await userManager.AddToRoleAsync(user, Roles.Passenger);
            }

            var token = await tokenService.GenerateAccessTokenAsync(user);

            return new AuthResultDto(token, DateTime.UtcNow.AddHours(12));
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null || user.Email == null) return;

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"https://gobet-app.com/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";

            await emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
        }

        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email)
                ?? throw new Exception("User not found");

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}