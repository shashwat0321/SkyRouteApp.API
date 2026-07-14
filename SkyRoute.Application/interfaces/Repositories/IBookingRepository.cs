using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task SaveChangesAsync();
    }
}
