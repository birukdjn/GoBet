using GoBet.Application.Services.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace GoBet.Application.Services.Implementations
{
    public class DriverService( UserManager<ApplicationUser> userManager) : IDriverService
    {
        public async Task RequestDriverAsync(string userId, string licenseNumber)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            user.LicenseNumber = licenseNumber;
            await userManager.UpdateAsync(user);
        }
        public async Task ApproveDriverAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            user.IsDriverApproved = true;
            var result = await userManager.AddToRoleAsync(user, Roles.Driver);

            if (!result.Succeeded)
                throw new Exception("Failed to assign driver role.");
        }
    }
}