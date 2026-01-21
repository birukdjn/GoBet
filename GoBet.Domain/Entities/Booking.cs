namespace GoBet.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string PassengerId { get; set; } = string.Empty;
        public Guid TripId { get; set; }

        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }

        public bool IsPickedUp { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        public Trip Trip { get; set; } = null!;

    }
}