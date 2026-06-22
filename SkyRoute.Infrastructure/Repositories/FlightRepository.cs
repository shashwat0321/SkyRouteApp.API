using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Domain.Entities;
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

        public async Task<Flight?> GetFlightAsync(int scheduleId, DateTime flightDate)
        {
            return await _context.Flights
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.Airline)
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.DepartureAirport)
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.FlightScheduleId == scheduleId && f.FlightDate.Date == flightDate.Date);
        }

        public async Task<Flight?> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.Airline)
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.DepartureAirport)
                .Include(f => f.FlightSchedule)
                    .ThenInclude(s => s.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == flightId);
        }

        public async Task<Flight> AddFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }
    }
}
