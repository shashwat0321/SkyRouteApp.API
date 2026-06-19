using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Domain.Entities;
using SkyRoute.Infrastructure.Data;

namespace SkyRoute.Infrastructure.Repositories
{
    public class FlightRepository: IFlightRepository
    {
       // private readonly IMapper _mapper;
        private readonly SkyRouteDbContext _context;
        public FlightRepository(SkyRouteDbContext context, IMapper mapper)
        {
            _context = context;
         //   _mapper = mapper;
        }
        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            return await _context.Flights
                                 .Include(f => f.Airline)
                                 .Include(f => f.DepartureAirport)
                                 .Include(f => f.ArrivalAirport)
                                 .ToListAsync();

        }
    }
}
