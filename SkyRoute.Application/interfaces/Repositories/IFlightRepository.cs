using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces.Repositories
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllFlightsAsync();
    }
}
