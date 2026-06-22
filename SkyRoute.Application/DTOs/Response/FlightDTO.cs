using System.ComponentModel.DataAnnotations;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.DTOs.Response
{
    public class FlightDTO
    {
        public int FlightId { get; set; }
        public int ScheduleId { get; set; }

        [Required]
        public string FlightNumber { get; set; } = null!;
        [Required]
        public string AirlineName { get; set; } = null!;
        [Required]
        public string AirlineCode { get; set; } = null!;

        [Required]
        public string DepartureAirportCode { get; set; } = null!;
        [Required]
        public string DepartureCity { get; set; } = null!;
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public string ArrivalAirportCode { get; set; } = null!;
        [Required]
        public string ArrivalCity { get; set; } = null!;
        public DateTime ArrivalDateTime { get; set; }

        public int EconomyAvailable { get; set; }
        public int BusinessAvailable { get; set; }
        public int FirstClassAvailable { get; set; }

        public decimal EconomyPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal FirstClassPrice { get; set; }

        public FlightStatus Status { get; set; }
    }
}
