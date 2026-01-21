
using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
    }
}
