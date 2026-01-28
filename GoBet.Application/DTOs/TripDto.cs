using GoBet.Domain.Entities;

namespace GoBet.Application.DTOs
{
    public record TripDto(
        Guid Id,
        string Destination,
        int TotalSeats,
        int AvailableSeats,
        string BusPlateNumber,
        double CurrentLatitude,
        double CurrentLongitude,
        string Status
    )
    {
        public TripDto(Trip trip) : this(
            trip.Id,
            trip.Destination,
            trip.TotalSeats,
            trip.AvailableSeats,
            trip.BusPlateNumber,
            trip.CurrentLatitude,
            trip.CurrentLongitude,
            trip.Status.ToString()
        )
        { }
    }

}
