

namespace GoBet.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int UsersCount { get; set; }
        public int AdminsCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int ActiveAdminsCount { get; set; }
        public int DriversCount { get; set; }
        public int ActiveDriversCount { get; set; } 
        public int PassengersCount { get; set; }
        public int ActivePassengersCount { get; set; }
        public int TripsCount { get; set; }
        public int ActiveTripsCount { get; set; }
        public int BookingsCount { get; set; }
    }
}
