using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<FlightSchedule>> SearchSchedulesAsync(string origin, string destination, DateOnly date);
        Task<FlightSchedule?> GetScheduleByIdAsync(int scheduleId);
    }
}
