using GoBet.Application.DTOs;

namespace GoBet.Application.Interfaces.Services
{
    public interface IPassengerService
    {
        Task<IEnumerable<TripDto>> FindNearbyBusesAsync(double lat, double lon, string destination);
        Task<NearbyBusesResponse> FindBusesAtNearestTerminalAsync(double lat, double lon, string destination);
        Task<BookingResponse> BookRoadsidePickupAsync(Guid tripId, string passengerId, double Latitude, double Longitude);
    }

}
