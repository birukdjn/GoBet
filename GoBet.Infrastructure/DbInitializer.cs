using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GoBet.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Create Roles
            string[] roles = { Roles.Admin, Roles.Driver, Roles.Passenger };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Create default Admin
            var adminEmail = "admin@gobet.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true // Good practice for seeded users
                };

                // Create the user first
                var createResult = await userManager.CreateAsync(admin, "Admin@gobet123!");

                if (createResult.Succeeded)
                {
                    // Now that we ARE SURE the user exists, add the role
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
                else
                {
                    // Log errors if creation failed (e.g. password too weak)
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create seed admin: {errors}");
                }
            }
        }


    }
}
