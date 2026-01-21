using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    }
}
