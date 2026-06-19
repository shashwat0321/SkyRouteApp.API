using SkyRoute.Domain.Common;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class FlightSchedule : BaseEntity
    {
        public string FlightNumber { get; set; }

        public int AirlineId { get; set; }
        public Airline Airline { get; set; }

        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }

        public int ArrivalAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }

        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }

        public OperatingDays OperatingDays { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
        public int FirstClassSeats { get; set; }

        public decimal EconomyBasePrice { get; set; }
        public decimal BusinessBasePrice { get; set; }
        public decimal FirstClassBasePrice { get; set; }

        public ScheduleStatus Status { get; set; }

        public List<Flight> Flights { get; set; }
    }
}
