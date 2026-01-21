using GoBet.Domain.Entities;

namespace GoBet.Application.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    }
}
