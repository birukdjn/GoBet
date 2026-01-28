using GoBet.Application.DTOs;
using GoBet.Application.Interfaces.Repositories;
using GoBet.Application.Interfaces.Services;
using GoBet.Domain.Entities;

namespace GoBet.Application.Services
{
    public class TripService(ITripRepository tripRepository
  ) : ITripService
    {
        public async Task<Trip?> GetTripByIdAsync(Guid tripId)
        {
            return await tripRepository.GetTripByIdAsync(tripId);
        }

        public async Task BookSeatAsync(Guid tripId, Guid userId)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId) ?? throw new Exception("Trip not found");

            trip.OccupySeat();

            await tripRepository.UpdateAsync(trip);

        }

        public async Task<IEnumerable<Trip>> GetActiveTripsByDestinationAsync(string destination)
        {
            return await tripRepository.GetActiveTripsByDestinationAsync(destination);
        }

        public async Task<Guid> CreateTripAsync(CreateTripRequest request, Guid driverId)
        {
            var trip = new Trip(
                driverId: driverId,
                destination: request.Destination,
                totalSeats: request.TotalSeats,
                routeId: request.RouteId,
                busPlateNumber: request.BusPlateNumber
            );

            await tripRepository.AddAsync(trip);
            return trip.Id;
        }

        public async Task UpdateLocationAsync(Guid tripId, Guid driverId, double latitude, double longitude)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId) ?? throw new Exception("Trip not found");

            if (trip.DriverId != driverId)
                throw new UnauthorizedAccessException("Driver cannot update location for this trip");

            trip.UpdateLocation(latitude, longitude);
            await tripRepository.UpdateAsync(trip);
        }

        public async Task StartTripAsync(Guid tripId, Guid driverId)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId) ?? throw new Exception("Trip not found");
            if (trip.DriverId != driverId)
            {
                throw new Exception("Driver not authorized to start this trip");
            }

            trip.StartTrip();
            await tripRepository.UpdateAsync(trip);
        }

        public async Task CompleteTripAsync(Guid tripId, Guid driverId)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId) ?? throw new Exception("Trip not found");

            if (trip.DriverId != driverId)
            {
                throw new Exception("Driver not authorized to complete this trip");
            }
            trip.CompleteTrip();
            await tripRepository.UpdateAsync(trip);
        }


    }
}

