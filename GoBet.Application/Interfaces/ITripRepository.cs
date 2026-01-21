using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip> GetByIdAsync(Guid id);
        Task UpdateAsync(Trip trip);
    }
}
