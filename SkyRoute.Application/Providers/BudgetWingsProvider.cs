using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Providers
{
    public class BudgetWingsProvider : IFlightProvider
    {
        public string AirlineCode => "BW";

        public decimal CalculatePrice(decimal basePrice, CabinClass cabinClass)
        {
            return Math.Max(basePrice * 0.90m, 29.99m);
        }
    }
}
