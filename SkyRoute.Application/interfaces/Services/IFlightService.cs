using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;

namespace SkyRoute.Application.Interfaces.Services
{
    public interface IFlightService
    {
        Task<FlightDTO> CreateFlightAsync(CreateFlightRequest request);
        Task<FlightDTO> GetFlightByIdAsync(int flightId);
        Task<FlightDTO> GetFlightByScheduleAsync(int scheduleId, DateOnly date);
    }
}
