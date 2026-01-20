using GoBet.Application.DTOs;
using Microsoft.AspNetCore.Http;
namespace GoBet.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterPassengerAsync(RegisterModel model);
        Task<AuthResultDto> LoginAsync(LoginModel model);
        Task<AuthResultDto> HandleExternalLoginAsync(HttpContext context);
    }
}
