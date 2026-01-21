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
            // Get all trips currently moving towards the desired destination
            var activeTrips = await tripRepository.GetActiveTripsByDestinationAsync(destination);

            // Filter trips that are within a 5km radius of the passenger's roadside position
            return activeTrips
                .Where(t => locationService.GetDistance(userLat, userLon, t.CurrentLatitude, t.CurrentLongitude) <= 5.0)
                .Select(t => new TripDto
                {
                    Id = t.Id,
                    PlateNumber = t.BusPlateNumber,
                    DistanceKm = locationService.GetDistance(userLat, userLon, t.CurrentLatitude, t.CurrentLongitude),
                    AvailableSeats = t.AvailableSeats
                });
        }

        public async Task<BookingResponse> BookRoadsidePickupAsync(BookingRequest request)
        {
            var trip = await tripRepository.GetByIdAsync(request.TripId)
                ?? throw new Exception("Bus not found");

            // Use the Domain method to decrease seat count
            trip.OccupySeat();

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                PassengerId = request.PassengerId,
                TripId = request.TripId,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                IsPickedUp = false
            };

            await bookingRepository.AddAsync(booking);
            await tripRepository.UpdateAsync(trip);

            return new BookingResponse
            {
                BookingId = booking.Id,
                Message = "Pickup confirmed! Please stay at your location."
            };
        }
    }
}