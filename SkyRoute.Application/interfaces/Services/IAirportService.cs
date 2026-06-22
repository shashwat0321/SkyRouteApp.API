using SkyRoute.Application.DTOs.Response;

namespace SkyRoute.Application.Interfaces.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportResponse>> GetAirportsAsync();
    }
}
