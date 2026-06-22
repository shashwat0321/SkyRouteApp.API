using Microsoft.AspNetCore.Mvc;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Services;

namespace SkyRoute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirportResponse>>> GetAllAirports()
        {
            var airports = await _airportService.GetAirportsAsync();
            return Ok(airports);
        }
    }
}
