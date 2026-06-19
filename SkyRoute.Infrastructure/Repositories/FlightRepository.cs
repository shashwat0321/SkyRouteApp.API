using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Infrastructure.Data;

namespace SkyRoute.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly SkyRouteDbContext _context;

        public FlightRepository(SkyRouteDbContext context)
        {
            _context = context;
        }
    }
}
