
namespace GoBet.Application.DTOs
{
    public class TripDto
    {
        public Guid Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
        public int AvailableSeats { get; set; }
    }
}
