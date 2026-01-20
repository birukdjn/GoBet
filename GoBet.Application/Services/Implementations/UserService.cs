using GoBet.Application.Services.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using GoBet.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoBet.Application.Services.Implementations
{
    public class UserService(UserManager<ApplicationUser> userManager) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IdentityResult> RegisterPassengerAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Passenger);
            }

            return result;
        }
        public async Task RequestDriverStatusAsync(string userId, string licenseNumber)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.LicenseNumber = licenseNumber;

            await _userManager.UpdateAsync(user);
        }

        public async Task ApproveDriverAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            user.IsDriverApproved = true;
            var result = await _userManager.AddToRoleAsync(user, Roles.Driver);

            if (!result.Succeeded)
                throw new Exception("Failed to assign driver role.");
        }

        public async Task<string?> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Generate Token (This is the "Key" for the mobile app)
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A_Very_Long_And_Secret_Key_With_At_Least_32_Chars!"));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        }
    } 
                    
}