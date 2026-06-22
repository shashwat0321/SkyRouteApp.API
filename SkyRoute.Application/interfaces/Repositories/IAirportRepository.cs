using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces.Repositories
{
    public interface IAirportRepository
    {
        Task <IEnumerable<Airport>> GetAllAirportsAsync();
    }
}
