using SkyRoute.Application.Interfaces.Services;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Repositories;
using AutoMapper;

namespace SkyRoute.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly IMapper _mapper;
        public FlightService(IFlightRepository flightRepo, IMapper mapper)
        {
            _flightRepo = flightRepo;
            _mapper = mapper;
        }
        public async Task<List<FlightResponseDTO>> GetAllFlightsAsync()
        {
            var flights = await _flightRepo.GetAllFlightsAsync();
            var flightDto = _mapper.Map<List<FlightResponseDTO>>(flights);
            return flightDto;
        }
    }
}
