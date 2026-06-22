using AutoMapper;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepo;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IFlightProvider> _providers;

        public ScheduleService(IScheduleRepository scheduleRepo, IMapper mapper, IEnumerable<IFlightProvider> providers)
        {
            _scheduleRepo = scheduleRepo;
            _mapper = mapper;
            _providers = providers;
        }

        public async Task<IEnumerable<ScheduleSearchResultDTO>> SearchSchedulesAsync(ScheduleSearchRequest request)
        {
            var schedules = (await _scheduleRepo.SearchSchedulesAsync(request.Origin, request.Destination, request.Date)).ToList();
            var dtos = _mapper.Map<List<ScheduleSearchResultDTO>>(schedules);
            var travelDate = request.Date.ToDateTime(TimeOnly.MinValue).Date;

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                dto.DepartureDateTime = travelDate.Add(dto.DepartureDateTime.TimeOfDay);
                dto.ArrivalDateTime = travelDate.Add(dto.ArrivalDateTime.TimeOfDay);

                if (dto.ArrivalDateTime <= dto.DepartureDateTime)
                    dto.ArrivalDateTime = dto.ArrivalDateTime.AddDays(1);

                var provider = _providers.First(p => p.AirlineCode == dto.AirlineCode);
                dto.EconomyPrice = provider.CalculatePrice(schedules[i].EconomyBasePrice, CabinClass.Economy);
            }

            return dtos;
        }
    }
}
