using AutoMapper;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;

namespace SkyRoute.Application.Services
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IMapper _mapper;

        public AirportService(IAirportRepository airportRepo, IMapper mapper)
        {
            _airportRepository = airportRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AirportResponse>> GetAirportsAsync()
        {
            var airports = await _airportRepository.GetAllAirportsAsync();
            return _mapper.Map<IEnumerable<AirportResponse>>(airports);
        }
    }
}
