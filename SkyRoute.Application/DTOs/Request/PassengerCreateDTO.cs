using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Application.DTOs.Request
{
    public class PassengerCreateDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string? PassportNumber { get; set; }

        public string? NationalId { get; set; }

        [Required]
        public string Nationality { get; set; } = null!;
    }
}
