using SkyRoute.Domain.Common;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public List<Passenger> Passengers { get; set; }
    }
}