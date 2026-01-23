
using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace GoBet.Application.Services
{
    public class AdminService(
        IEmailService emailService,
        UserManager<ApplicationUser> userManager) : IAdminService
    {
        public async Task ApproveDriverAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
            user.IsDriverApproved = true;
            var result = await userManager.AddToRoleAsync(user, Roles.Driver);

            if (!result.Succeeded)
                throw new Exception("Failed to assign driver role.");

            var subject = "GoBet - Driver Application Approved!";
            var body = $@"
                <h3>Congratulations, {user.FullName}!</h3>
                <p>Your driver application for GoBet Transport has been approved.</p>
                <p>You can now log in to your dashboard and start accepting passenger requests.</p>
                <br>
                <p>Safe travels,<br>The GoBet Team</p>";
            await emailService.SendEmailAsync(user.Email!, subject, body);
        }

    }
}
