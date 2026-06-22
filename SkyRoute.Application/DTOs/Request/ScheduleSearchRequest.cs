using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Application.DTOs.Request
{
    public class ScheduleSearchRequest
    {
        [Required]
        public string Origin { get; set; } = null!;

        [Required]
        public string Destination { get; set; } = null!;

        [Required]
        public DateOnly Date { get; set; }
    }
}
