using SkyRoute.Domain.Common;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public string FlightNumber { get; set; }

        public int AirlineId { get; set; }
        public Airline Airline { get; set; }  // NOT string

        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }

        public int ArrivalAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        // Start with Economy only
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal Price { get; set; }

        // TODO Phase 2: Add BusinessSeats, BusinessPrice

        public FlightStatus Status { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
