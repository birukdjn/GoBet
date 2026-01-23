using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip?> GetByIdAsync(Guid id);
        Task UpdateAsync(Trip trip);
        Task AddAsync(Trip trip); 
        Task<IEnumerable<Trip>> GetActiveTripsByDestinationAsync(string destination);
        Task<IEnumerable<Trip>> GetTripsPassingThroughTerminalAsync(Guid terminalId, string destination);
    }
}
