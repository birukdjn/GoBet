using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Repositories
{
    public interface ITripRepository
    {
        Task<Trip?> GetTripByIdAsync(Guid id);
        Task UpdateAsync(Trip trip);
        Task AddAsync(Trip trip);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Trip>> GetActiveTripsByDestinationAsync(string destination);
        Task<IEnumerable<Trip>> GetTripsPassingThroughTerminalAsync(Guid terminalId, string destination);
    }
}
