using Microsoft.AspNetCore.Mvc;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Services;

namespace SkyRoute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDTO>> GetFlightByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _flightService.GetFlightByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{scheduleId}/{date}")]
        public async Task<ActionResult<FlightDTO>> GetFlightByScheduleAsync(int scheduleId, DateOnly date)
        {
            var result = await _flightService.GetFlightByScheduleAsync(scheduleId, date);
            return Ok(result);
        }

        // TODO: Add [Authorize(Roles = "Admin")] when JWT authentication is implemented
        [HttpPost("create")]
        public async Task<ActionResult<FlightDTO>> CreateFlightAsync([FromBody] CreateFlightRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _flightService.CreateFlightAsync(request);
            return StatusCode(201, result);
        }
    }
}
