using Microsoft.AspNetCore.Mvc;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Services;

namespace SkyRoute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookingAsync([FromBody] BookingCreateDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookingService.CreateBookingAsync(request);
            return StatusCode(201, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingByIdAsync(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingsByUserIdAsync([FromQuery] int userId)
        {
            if (userId <= 0)
                return BadRequest("A valid userId must be provided.");

            var results = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(results);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBookingAsync(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return Ok(result);
        }
    }
}
