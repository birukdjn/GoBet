
using GoBet.Application.DTOs;

namespace GoBet.Application.Interfaces
{
    public interface IPassengerService
    {
        Task<IEnumerable<TripDto>> FindNearbyBusesAsync(double userLat, double userLon, string destination);
        Task<BookingResponse> BookRoadsidePickupAsync(BookingRequest request);
        Task<NearbyBusesResponse> FindBusesAtNearestTerminalAsync(double pLat, double pLon, string destination);
    }
}

