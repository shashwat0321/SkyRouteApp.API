using SkyRoute.Domain.Common;

namespace SkyRoute.Domain.Entities
{
    public class Airline : BaseEntity
    {
        public string Name { get; set; }      // "GlobalWings"
        public string Code { get; set; }      // "GW"
        public List<Flight> Flights { get; set; }
    }
}