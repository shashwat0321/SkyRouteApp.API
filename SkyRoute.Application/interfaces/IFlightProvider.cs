using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Interfaces
{
    public interface IFlightProvider
    {
        string AirlineCode { get; }
        decimal CalculatePrice(decimal basePrice, CabinClass cabinClass);
    }
}
