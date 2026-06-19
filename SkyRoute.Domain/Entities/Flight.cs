using SkyRoute.Domain.Common;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public int FlightScheduleId { get; set; }
        public FlightSchedule FlightSchedule { get; set; }

        public DateTime FlightDate { get; set; }

        public int EconomyAvailable { get; set; }
        public int BusinessAvailable { get; set; }
        public int FirstClassAvailable { get; set; }

        public FlightStatus Status { get; set; }

        // Concurrency token — used to prevent duplicate instance creation / overbooking
        public byte[] RowVersion { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
