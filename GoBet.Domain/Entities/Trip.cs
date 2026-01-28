using GoBet.Domain.Constants;

namespace GoBet.Domain.Entities
{
    public class Trip
    {
        private readonly List<RoadTerminal> _routeStops = [];

        private Trip() { }

        public Trip(Guid driverId, string destination, int totalSeats, Guid routeId, string busPlateNumber)
        {
            if (driverId == Guid.Empty)
                throw new ArgumentException("DriverId is required.");

            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Destination is required.");

            if (totalSeats <= 0)
                throw new ArgumentException("Total seats must be greater than zero.");

            if (routeId == Guid.Empty)
                throw new ArgumentException("RouteId is required.");

            if (string.IsNullOrWhiteSpace(busPlateNumber))
                throw new ArgumentException("Bus plate number is required.");

            Id = Guid.NewGuid();
            DriverId = driverId;
            Destination = destination;
            RouteId = routeId;
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats;
            BusPlateNumber = busPlateNumber;
            Status = TripStatus.Scheduled;
        }

        public Guid Id { get; private set; }
        public Guid DriverId { get; private set; }
        public string Destination { get; private set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }
        public bool IsFull => AvailableSeats == 0;
        public string BusPlateNumber { get; private set; }
        public Guid RouteId { get; private set; }
        public double CurrentLatitude { get; private set; }
        public double CurrentLongitude { get; private set; }
        public TripStatus Status { get; private set; }
        public IReadOnlyCollection<RoadTerminal> RouteStops => _routeStops.AsReadOnly();

        // ---------- Domain Behavior ----------

        public void OccupySeat()
        {
            if (IsFull)
                throw new InvalidOperationException("No seats available.");

            AvailableSeats--;
        }

        public void ReleaseSeat()
        {
            if (AvailableSeats >= TotalSeats)
                throw new InvalidOperationException("All seats are already free.");

            AvailableSeats++;
        }

        public void UpdateLocation(double latitude, double longitude)
        {
            if (latitude is < -90 or > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude));

            if (longitude is < -180 or > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude));

            CurrentLatitude = latitude;
            CurrentLongitude = longitude;
        }

        public void StartTrip()
        {
            if (Status != TripStatus.Scheduled)
                throw new InvalidOperationException("Trip cannot be started.");

            Status = TripStatus.InProgress;
        }

        public void CompleteTrip()
        {
            if (Status != TripStatus.InProgress)
                throw new InvalidOperationException("Trip cannot be completed.");

            Status = TripStatus.Completed;
        }

        public void AddStop(RoadTerminal stop)
        {
            ArgumentNullException.ThrowIfNull(stop);

            _routeStops.Add(stop);
        }
    }
}
