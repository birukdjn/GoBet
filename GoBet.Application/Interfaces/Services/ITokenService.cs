using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    }
}
