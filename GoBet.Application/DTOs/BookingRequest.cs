
namespace GoBet.Application.DTOs
{
    public class BookingRequest
    {
        public Guid TripId { get; set; }
        public string? PassengerId { get; set; } // Filled by Controller
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
    }
}
