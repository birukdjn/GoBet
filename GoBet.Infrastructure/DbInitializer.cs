using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

<<<<<<< HEAD
namespace GoBet.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { Roles.Admin, Roles.Driver, Roles.Passenger })
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var adminEmail = "admin@gobet.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                FullName = "System Admin",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin@gobet123!");
            await userManager.AddToRoleAsync(admin, Roles.Admin);
        }
=======
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


>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
    }
}
