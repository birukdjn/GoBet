using GoBet.Application.DTOs;
using Microsoft.AspNetCore.Http;
namespace GoBet.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterPassengerAsync(RegisterModel model);
        Task<AuthResultDto> LoginAsync(LoginModel model);
        Task<AuthResultDto> HandleExternalLoginAsync(HttpContext context);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordModel model);
    }
}
