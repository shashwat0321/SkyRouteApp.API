using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Domain.Entities;
using SkyRoute.Infrastructure.Data;

namespace SkyRoute.Infrastructure.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly SkyRouteDbContext _context;

        public AirportRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airport>> GetAllAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }
    }
}
