using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;

namespace SkyRoute.Application.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleSearchResultDTO>> SearchSchedulesAsync(ScheduleSearchRequest request);
    }
}
