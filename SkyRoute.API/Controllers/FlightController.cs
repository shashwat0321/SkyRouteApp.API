

using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<List<FlightResponseDTO>>> GetAllFlightsAsync()
        {
            var flights = await _flightService.GetAllFlightsAsync();

            return Ok(flights);
        }
    }
}
