using SkyRoute.Application.DTOs.Response;

namespace SkyRoute.Application.Interfaces.Services
{
    public interface IFlightService
    {
        Task<List<FlightResponseDTO>> GetAllFlightsAsync();
    }
}
