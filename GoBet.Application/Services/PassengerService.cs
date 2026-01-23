using GoBet.Application.Interfaces;
using GoBet.Domain.Entities;
using GoBet.Application.DTOs;
using Microsoft.AspNetCore.SignalR;
using GoBet.Application.Hubs;

namespace GoBet.Application.Services
{
    public class PassengerService(
        ITripRepository tripRepository,
        IBookingRepository bookingRepository,
        ITerminalRepository terminalRepository,
        ILocationService locationService,
        IHubContext<TripHub> hubContext) : IPassengerService
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

            await hubContext.Clients.Group(request.TripId.ToString())
            .SendAsync("ReceivePickupRequest", new
            {
                request.PickupLatitude,
                request.PickupLongitude,
                PassengerId = request.PassengerId,
                Message = "New roadside passenger waiting!"
            });

            return new BookingResponse
            {
                BookingId = booking.Id,
                Message = $"Confirmed! Bus {trip.BusPlateNumber} is informed. Wait at your current spot."
            };

        }

        public async Task<NearbyBusesResponse> FindBusesAtNearestTerminalAsync(double pLat, double pLon, string destination)
        {
            // 1. Get all terminals
            var allTerminals = await terminalRepository.GetAllAsync();

            // 2. Find the one closest to the passenger
            var nearest = locationService.GetNearestTerminal(pLat, pLon, allTerminals);

            // 3. Find trips that are passing through this specific terminal
            var trips = await tripRepository.GetTripsPassingThroughTerminalAsync(nearest.Id, destination);

            return new NearbyBusesResponse
            {
                NearestTerminalName = nearest.Name,
                Buses = trips.Select(t => new TripDto
                {
                    Id = t.Id,
                    PlateNumber = t.BusPlateNumber,
                    AvailableSeats = t.AvailableSeats,
                    // Calculate distance to the TERMINAL, not the bus
                    DistanceKm = Math.Round(locationService.GetDistance(pLat, pLon, nearest.Latitude, nearest.Longitude), 2)
                })
            };
        }
    }
}