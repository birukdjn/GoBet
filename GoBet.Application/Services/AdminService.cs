using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Repositories;
using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace GoBet.Application.Services
{
    public class AdminService(
        IBookingRepository bookingRepository,
        IEmailService emailService,
        ITripRepository tripRepository,
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

        public async Task<IEnumerable<UserDetailDto>> GetAllUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userList = new List<UserDetailDto>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var isLockedOut = await userManager.IsLockedOutAsync(user);
                userList.Add(new UserDetailDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? "Passenger",
                    Status = isLockedOut ? "Inactive" : "Active"
                });
            }

            return userList;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var users = await userManager.Users.ToListAsync();

            return new DashboardStatsDto
            {
                UsersCount = users.Count,
                DriversCount = users.Count(u => u.IsDriverApproved),
                PassengersCount = users.Count(u => !u.IsDriverApproved),
                TripsCount = (await tripRepository.GetAllAsync()).Count(),
                BookingsCount = (await bookingRepository.GetAllAsync()).Count(),
                ActiveDriversCount = await tripRepository.GetActiveTripsCountAsync(),
                ActiveUsersCount = users.Count(u => u.LastLoginDate > DateTime.UtcNow.AddDays(-7))
            };
        }

        public async Task ChangeUserRoleAsync(string userId, string newRole)
{
    var user = await userManager.FindByIdAsync(userId) 
               ?? throw new Exception("User not found");

    // Get current roles and remove them
    var currentRoles = await userManager.GetRolesAsync(user);
    await userManager.RemoveFromRolesAsync(user, currentRoles);

    // Add the new role
    var result = await userManager.AddToRoleAsync(user, newRole);
    if (!result.Succeeded) 
        throw new Exception("Failed to update role");
}

public async Task UpdateUserStatusAsync(string userId, string status)
{
    var user = await userManager.FindByIdAsync(userId) 
               ?? throw new Exception("User not found");

    // Logic for 'Inactive' can be mapped to lockout or a custom IsActive property
    if (status == "Inactive")
    {
        // Example: Lock the account for 100 years
        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
    }
    else
    {
        // Re-activate by clearing lockout
        await userManager.SetLockoutEndDateAsync(user, null);
    }
}

    }
}
