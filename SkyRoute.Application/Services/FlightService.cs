using AutoMapper;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;

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
    }
}
