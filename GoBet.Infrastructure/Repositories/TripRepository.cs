using GoBet.Application.Interfaces;
using GoBet.Domain.Constants;
using GoBet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoBet.Infrastructure.Repositories
{
    public class TripRepository(ApplicationDbContext context) : ITripRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Trip>> GetActiveTripsByDestinationAsync(string destination)
        {
            return await _context.Trips
                .Where(t => t.Destination == destination &&
                            t.Status == TripStatus.EnRoute &&
                            t.AvailableSeats > 0)
                .ToListAsync();
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task UpdateAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }
    }
}