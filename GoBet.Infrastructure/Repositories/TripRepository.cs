using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoBet.Infrastructure.Repositories
{
    public class TripRepository(ApplicationDbContext context) : ITripRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<IEnumerable<Trip>> GetActiveTripsByDestinationAsync(string destination)
        {
            return await context.Trips
                .Where(t => t.Destination == destination &&
                            t.Status == TripStatus.EnRoute &&
                            t.AvailableSeats > 0)
                .ToListAsync();
        }

        public async Task AddAsync(Trip trip)
        {
            await context.Trips.AddAsync(trip);
            await context.SaveChangesAsync();
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
        {
            return await context.Trips.FindAsync(id);
        }

        public async Task UpdateAsync(Trip trip)
        {
            context.Trips.Update(trip);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsPassingThroughTerminalAsync(Guid terminalId, string destination)
        {
            return await context.Trips
                .Include(t => t.RouteStops)
                .Where(t => t.Destination == destination &&
                            t.Status == TripStatus.EnRoute &&
                            t.RouteStops.Any(s => s.Id == terminalId))
                .ToListAsync();
        }
    }
}