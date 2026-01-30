using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetAllAsync();
    }
}
