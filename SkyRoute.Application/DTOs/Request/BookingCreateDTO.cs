using System.ComponentModel.DataAnnotations;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.DTOs.Request
{
    public class BookingCreateDTO
    {
        [Required]
        public int FlightId { get; set; }

        [Required]
        public CabinClass CabinClass { get; set; }

        [Required]
        public List<PassengerCreateDTO> Passengers { get; set; } = null!;
    }
}
