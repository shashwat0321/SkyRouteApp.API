using SkyRoute.Domain.Common;

namespace SkyRoute.Domain.Entities
{
    public class Passenger : BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string PassportNumber { get; set; }    // Nullable
        public string NationalId { get; set; }        // Nullable
        public string Nationality { get; set; }
    }
}