using GoBet.Domain.Constants;

namespace GoBet.Domain.Entities
{
    public class Trip
    {
        // EF Core needs this empty constructor
        public Trip() { }

        // You can still have a logic-based constructor if you want
        public Trip(int totalSeats)
        {
            Id = Guid.NewGuid();
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats;
        }

        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public string Destination { get; set; } = string.Empty;

        // Map the capacity to the database so EF knows what 'totalSeats' was
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
    }
}