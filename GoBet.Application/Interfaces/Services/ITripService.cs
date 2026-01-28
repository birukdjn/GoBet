using GoBet.Application.DTOs;
using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Services
{
    public interface ITripService
    {
        Task<Trip?> GetTripByIdAsync(Guid tripId);
        Task StartTripAsync(Guid tripId, Guid driverId);
        Task CompleteTripAsync(Guid tripId, Guid driverId);
        Task<Guid> CreateTripAsync(CreateTripRequest request, Guid driverId);
        Task UpdateLocationAsync(Guid tripId, Guid driverId, double lat, double lon);
    }
}

