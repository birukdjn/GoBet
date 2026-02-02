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

            // Prevent submission if already approved or if a request is already pending
            if (user.IsDriverApproved)
                throw new Exception("You are already an approved driver.");

            if (!string.IsNullOrEmpty(user.LicenseNumber))
                throw new Exception("You already have a pending driver request.");

            user.LicenseNumber = licenseNumber;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Failed to submit request.");
        }

        public async Task<string> GetRequestStatusAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            if (user.IsDriverApproved) return "Approved";

            // If they have a license number but aren't approved, it's pending
            if (!string.IsNullOrEmpty(user.LicenseNumber)) return "Pending";

            return "None";
        }
    }
}