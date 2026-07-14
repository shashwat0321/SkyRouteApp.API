using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;

namespace SkyRoute.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDTO> CreateBookingAsync(BookingCreateDTO request);
        Task<BookingResponseDTO> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingResponseDTO>> GetBookingsByUserIdAsync(int userId);
        Task<BookingResponseDTO> CancelBookingAsync(int bookingId);
    }
}
