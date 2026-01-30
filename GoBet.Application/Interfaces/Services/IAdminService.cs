using GoBet.Application.DTOs;
using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task ApproveDriverAsync (string userId);
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<UserDetailDto>> GetAllUsersAsync();
        Task ChangeUserRoleAsync(string userId, string newRole);
        Task UpdateUserStatusAsync(string userId, string status);
    }
}
