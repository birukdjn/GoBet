using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Services
{
    public interface ILocationService
    {
        double GetDistance(double lat1, double lon1, double lat2, double lon2);

        bool IsWithinPickupRange(double passengerLat, double passengerLon, double busLat, double busLon, double radiusInKm = 1.0);

        RoadTerminal GetNearestTerminal(double pLat, double pLon, IEnumerable<RoadTerminal> terminals);
    }
}
