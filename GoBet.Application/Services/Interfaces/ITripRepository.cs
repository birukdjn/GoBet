using GoBet.Domain.Entities;

namespace GoBet.Application.Services.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip> GetByIdAsync(Guid id);
        Task UpdateAsync(Trip trip);
    }
}
