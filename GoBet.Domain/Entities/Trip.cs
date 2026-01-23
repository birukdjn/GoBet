using GoBet.Domain.Constants;

namespace GoBet.Domain.Entities
{
    public class Trip
    {
        public Trip() { }

        public Trip(int totalSeats)
        {
            Id = Guid.NewGuid();
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats;
        }

        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public string Destination { get; set; } = string.Empty;

        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }

        public bool IsFull => AvailableSeats <= 0;

        public void OccupySeat()
        {
            if (IsFull) throw new Exception("No seats available.");
            AvailableSeats--;
        }

        public string BusPlateNumber { get; set; } = string.Empty;
        public Guid RouteId { get; set; }

        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public TripStatus Status { get; set; }

        public List<RoadTerminal> RouteStops { get; set; } = new();
    }
}