using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Providers
{
    public class GlobalWingsProvider : IFlightProvider
    {
        public string AirlineCode => "GW";

        public decimal CalculatePrice(decimal basePrice, CabinClass cabinClass)
        {
            return Math.Round(basePrice * 1.15m, 2);
        }
    }
}
