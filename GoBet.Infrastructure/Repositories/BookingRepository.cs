using GoBet.Application.Interfaces.Repositories;
using GoBet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoBet.Infrastructure.Repositories
{
    public class BookingRepository(ApplicationDbContext context) : IBookingRepository
    {
        public async Task AddAsync(Booking booking)
        {
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await context.Bookings
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
