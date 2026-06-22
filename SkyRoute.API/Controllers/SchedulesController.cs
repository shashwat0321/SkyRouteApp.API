using Microsoft.AspNetCore.Mvc;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Services;

namespace SkyRoute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ScheduleSearchResultDTO>>> SearchSchedulesAsync([FromQuery] ScheduleSearchRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var results = await _scheduleService.SearchSchedulesAsync(request);

            if (!results.Any())
                return NotFound("No schedules found for the given route and date.");

            return Ok(results);
        }
    }
}
