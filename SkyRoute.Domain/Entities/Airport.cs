using SkyRoute.Domain.Common;

namespace SkyRoute.Domain.Entities
{
    public class Airport : BaseEntity
    {
        public string Code { get; set; }      // IATA code: "DEL", "BOM"
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}