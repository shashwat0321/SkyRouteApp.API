using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Common.Helpers;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Domain.Entities;
using SkyRoute.Infrastructure.Data;

namespace SkyRoute.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly SkyRouteDbContext _context;

        public ScheduleRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightSchedule>> SearchSchedulesAsync(string origin, string destination, DateOnly date)
        {
            var travelDate = date.ToDateTime(TimeOnly.MinValue);
            var operatingDay = DateHelper.ConvertDayToFlag(travelDate.DayOfWeek);

            return await _context.FlightSchedules
                .Include(f => f.Airline)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Where(f => f.DepartureAirport.Code == origin
                            && f.ArrivalAirport.Code == destination)
                .Where(f => travelDate.Date >= f.EffectiveFrom.Date && travelDate.Date <= f.EffectiveTo.Date)
                .Where(f => (f.OperatingDays & operatingDay) != 0)
                .ToListAsync();
        }

        public async Task<FlightSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _context.FlightSchedules
                .Include(s => s.Airline)
                .Include(s => s.DepartureAirport)
                .Include(s => s.ArrivalAirport)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);
        }
    }
}
