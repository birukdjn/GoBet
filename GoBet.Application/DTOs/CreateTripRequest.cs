namespace GoBet.Application.DTOs
{
    public class CreateTripRequest
    {
        public string Destination { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public Guid RouteId { get; set; }
        public string BusPlateNumber { get; set; } = string.Empty;
    }
}
