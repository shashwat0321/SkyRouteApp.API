using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.DTOs.Response
{
    public class BookingResponseDTO
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public CabinClass CabinClass { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
