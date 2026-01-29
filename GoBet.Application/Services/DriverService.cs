using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace GoBet.Application.Services
{
    public class DriverService( 
        UserManager<ApplicationUser> userManager) : IDriverService
    {
        public async Task RequestDriverAsync(string userId, string licenseNumber)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            user.LicenseNumber = licenseNumber;
            await userManager.UpdateAsync(user);
        }
    }
}