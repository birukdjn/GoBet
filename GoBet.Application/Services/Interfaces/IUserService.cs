using GoBet.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GoBet.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterPassengerAsync(RegisterModel model);
        Task RequestDriverStatusAsync(string userId, string licenseNumber);
        Task ApproveDriverAsync(string userId);
        Task<string?> LoginAsync(LoginModel model);
    }
}
