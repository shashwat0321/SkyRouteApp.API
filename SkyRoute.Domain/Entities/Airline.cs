using SkyRoute.Domain.Common;

namespace SkyRoute.Domain.Entities
{
    public class Airline : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public List<FlightSchedule> Schedules { get; set; }
    }
}
