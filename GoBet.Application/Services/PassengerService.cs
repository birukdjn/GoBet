using GoBet.Application.Interfaces;
using GoBet.Domain.Entities;
using GoBet.Application.DTOs;

namespace GoBet.Application.Services
{
    public class PassengerService(
        ITripRepository tripRepository,
        IBookingRepository bookingRepository,
        ILocationService locationService) : IPassengerService
    {
        public async Task<IEnumerable<TripDto>> FindNearbyBusesAsync(double userLat, double userLon, string destination)
        {
            // 1. Get all active trips for the destination (e.g., Piassa)
            var activeTrips = await tripRepository.GetActiveTripsByDestinationAsync(destination);

            // 2. Filter by distance (5km) and map to DTO
            return activeTrips
                .Select(t => new {
                    Trip = t,
                    Distance = locationService.GetDistance(userLat, userLon, t.CurrentLatitude, t.CurrentLongitude)
                })
                .Where(x => x.Distance <= 5.0)
                .OrderBy(x => x.Distance) // Show closest bus first
                .Select(x => new TripDto
                {
                    Id = x.Trip.Id,
                    PlateNumber = x.Trip.BusPlateNumber,
                    DistanceKm = Math.Round(x.Distance, 2), // Round for cleaner UI
                    AvailableSeats = x.Trip.AvailableSeats
                });
        }

        public async Task<BookingResponse> BookRoadsidePickupAsync(BookingRequest request)
        {
            // 1. Validate inputs
            if (string.IsNullOrEmpty(request.PassengerId))
                throw new Exception("User identification failed.");

            var trip = await tripRepository.GetByIdAsync(request.TripId)
                ?? throw new Exception("The selected bus is no longer available.");

            // 2. Logic Check: Does the bus have room?
            // This calls the method in our Domain Entity
            trip.OccupySeat();

            // 3. Create the Booking
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                PassengerId = request.PassengerId,
                TripId = request.TripId,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                IsPickedUp = false,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Persistence (Database Transaction-like)
            await bookingRepository.AddAsync(booking);
            await tripRepository.UpdateAsync(trip);

            return new BookingResponse
            {
                BookingId = booking.Id,
                Message = $"Confirmed! Bus {trip.BusPlateNumber} is informed. Wait at your current spot."
            };
        }
    }
}