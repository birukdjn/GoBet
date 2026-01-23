using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace GoBet.Application.Services
{
    public class DriverService( 
        UserManager<ApplicationUser> userManager, 
        ITripRepository tripRepository,
    ITerminalRepository terminalRepository) : IDriverService
    {
        public async Task RequestDriverAsync(string userId, string licenseNumber)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            user.LicenseNumber = licenseNumber;
            await userManager.UpdateAsync(user);
        }
        
        public async Task<Guid> StartTripAsync(string driverId, string destination, List<Guid> terminalIds)
        {
            // Fetch the actual terminal data from DB
            var terminals = await terminalRepository.GetByIdsAsync(terminalIds);

            var newTrip = new Trip(30) // Assuming 30 seats
            {
                DriverId = Guid.Parse(driverId),
                Destination = destination,
                Status = TripStatus.EnRoute,
                RouteStops = terminals.ToList(),
                CurrentLatitude = terminals.First().Latitude,
                CurrentLongitude = terminals.First().Longitude
            };

            await tripRepository.AddAsync(newTrip);
            return newTrip.Id;
        }
    }
}