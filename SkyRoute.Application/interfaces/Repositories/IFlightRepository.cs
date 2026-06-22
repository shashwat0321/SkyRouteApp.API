using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces.Repositories
{
    public interface IFlightRepository
    {
        Task<Flight?> GetFlightAsync(int scheduleId, DateTime flightDate);
        Task<Flight?> GetFlightByIdAsync(int flightId);
        Task<Flight> AddFlightAsync(Flight flight);
    }
}
