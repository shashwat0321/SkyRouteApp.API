using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Application.DTOs.Response
{
    public class ScheduleSearchResultDTO
    {
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

        public decimal EconomyPrice { get; set; }
    }
}
