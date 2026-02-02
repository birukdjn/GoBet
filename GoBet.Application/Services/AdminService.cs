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
                userList.Add(new UserDetailDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? "Passenger",
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate,
                });
            }

            return userList;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();
            var drivers = await userManager.GetUsersInRoleAsync(Roles.Driver);
            var passengers = await userManager.GetUsersInRoleAsync(Roles.Passenger);
            var admin = await userManager.GetUsersInRoleAsync(Roles.Admin);

            return new DashboardStatsDto
            {
                UsersCount = allUsers.Count,
                AdminsCount = admin.Count,
                DriversCount = drivers.Count,
                PassengersCount = passengers.Count,
                ActiveUsersCount = allUsers.Count(u => u.IsActive),
                ActiveAdminsCount = admin.Count(u => u.IsActive),
                ActiveDriversCount = drivers.Count(u => u.IsActive),
                ActivePassengersCount = passengers.Count(u => u.IsActive),

                TripsCount = (await tripRepository.GetAllAsync()).Count(),
                BookingsCount = (await bookingRepository.GetAllAsync()).Count(),
                ActiveTripsCount = await tripRepository.GetActiveTripsCountAsync(),
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

        public async Task<IEnumerable<DriverRequestDetailDto>> GetPendingDriverRequestsAsync()
        {
            // Fetch users who are NOT approved yet but have a license number (indicating they applied)
            var pendingUsers = await userManager.Users
                .Where(u => !u.IsDriverApproved && !string.IsNullOrEmpty(u.LicenseNumber))
                .ToListAsync();

            return pendingUsers.Select(u => new DriverRequestDetailDto
            {
                UserId = u.Id,
                FullName = u.FullName,
                Email = u.Email!,
                LicenseNumber = u.LicenseNumber!,
                RequestedAt = DateTime.UtcNow // If you add a RequestedDate to your User entity, use that here
            });
        }

        public async Task RejectDriverAsync(string userId, string reason)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");

            // Clear license info so they can re-apply if it was just a typo/bad image
            user.LicenseNumber = null;
            user.IsDriverApproved = false;

            await userManager.UpdateAsync(user);

            var subject = "GoBet - Driver Application Update";
            var body = $@"
        <h3>Hello, {user.FullName}</h3>
        <p>Unfortunately, your driver application could not be approved at this time.</p>
        <p><strong>Reason:</strong> {reason}</p>
        <p>You may re-apply from your dashboard after addressing the issue above.</p>";

            await emailService.SendEmailAsync(user.Email!, subject, body);
        }

        public async Task UpdateUserStatusAsync(string userId)
        {
            
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new Exception("User not found");

            bool isAdmin = await userManager.IsInRoleAsync(user, Roles.Admin);

            if (isAdmin && user.IsActive)
            {
                throw new Exception("Cannot deactivate an account with Administrator privileges.");
            }
            user.IsActive = !user.IsActive;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Update failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

        }
    }
}
