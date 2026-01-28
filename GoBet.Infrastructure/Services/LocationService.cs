using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Entities;

namespace GoBet.Infrastructure.Services
{
    public class LocationService : ILocationService
    {
        public double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in km
        }

        public RoadTerminal GetNearestTerminal(double pLat, double pLon, IEnumerable<RoadTerminal> terminals)
        {
            return terminals
                .OrderBy(t => GetDistance(pLat, pLon, t.Latitude, t.Longitude))
                .First();
        }

        public bool IsWithinPickupRange(double pLat, double pLon, double bLat, double bLon, double radius)
        {
            return GetDistance(pLat, pLon, bLat, bLon) <= radius;
        }

        private double ToRadians(double deg) => deg * (Math.PI / 180);
    }
}