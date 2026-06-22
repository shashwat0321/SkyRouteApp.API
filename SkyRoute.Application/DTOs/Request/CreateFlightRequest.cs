using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Application.DTOs.Request
{
    public class CreateFlightRequest
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public DateOnly FlightDate { get; set; }
    }
}
