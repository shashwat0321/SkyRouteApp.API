using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Domain.Entities;
using SkyRoute.Infrastructure.Data;

namespace SkyRoute.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SkyRouteDbContext _context;

        public BookingRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
