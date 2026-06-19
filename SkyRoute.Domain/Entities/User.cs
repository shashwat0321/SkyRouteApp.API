using SkyRoute.Domain.Common;

namespace SkyRoute.Domain.Entities
{
    public class User : BaseEntity  // Gets Id, CreatedAt, UpdatedAt
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
