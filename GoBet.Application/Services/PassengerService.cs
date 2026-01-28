using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Repositories;
using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Entities;
namespace GoBet.Application.Services
{
    public class PassengerService(
    ITripRepository tripRepository,
    IBookingRepository bookingRepository,
    ITerminalRepository terminalRepository,
    ILocationService locationService,
    INotificationService notificationService
) : IPassengerService
    {
        public async Task<IEnumerable<TripDto>> FindNearbyBusesAsync(double lat, double lon, string destination)
        {
            var trips = await tripRepository.GetActiveTripsByDestinationAsync(destination);

            return trips
                .Select(t => new { Trip = t, Distance = locationService.GetDistance(lat, lon, t.CurrentLatitude, t.CurrentLongitude) })
                .Where(x => x.Distance <= 5)
                .OrderBy(x => x.Distance)
                .Select(x => new TripDto(x.Trip));
        }

        public async Task<NearbyBusesResponse> FindBusesAtNearestTerminalAsync(double lat, double lon, string destination)
        {
            var terminals = await terminalRepository.GetAllAsync();
            var nearest = locationService.GetNearestTerminal(lat, lon, terminals);

            var trips = await tripRepository.GetTripsPassingThroughTerminalAsync(nearest.Id, destination);

            return new NearbyBusesResponse
            {
                NearestTerminalName = nearest.Name,
                Buses = trips.Select(t => new TripDto(t))
            };
        }

        public async Task<BookingResponse> BookRoadsidePickupAsync(Guid tripId, string passengerId, double latitude, double longtude)
        {
            if (string.IsNullOrWhiteSpace(passengerId))
                throw new UnauthorizedAccessException("Passenger not authenticated.");

            var trip = await tripRepository.GetTripByIdAsync(tripId)
                       ?? throw new Exception("Trip not found");

            trip.OccupySeat();

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                PassengerId = passengerId,
                TripId = tripId,
                PickupLatitude =latitude,
                PickupLongitude =longtude,
                CreatedAt = DateTime.UtcNow
            };

            await bookingRepository.AddAsync(booking);
            await tripRepository.UpdateAsync(trip);

            await notificationService.NotifyDriverPickupRequestAsync(tripId, new
            {
                PickupLatitude = latitude,
                PickupLongitude = longtude,
                PassengerId = passengerId,
                Message = "New roadside passenger waiting!"
            });

            return new BookingResponse
            {
                BookingId = booking.Id,
                Message = $"Confirmed! Bus {trip.BusPlateNumber} is informed."
            };

        }
    }
}
