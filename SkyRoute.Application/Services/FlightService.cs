using AutoMapper;
using SkyRoute.Application.Common.Helpers;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly IScheduleRepository _scheduleRepo;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IFlightProvider> _providers;

        public FlightService(IFlightRepository flightRepo, IScheduleRepository scheduleRepo, IMapper mapper, IEnumerable<IFlightProvider> providers)
        {
            _flightRepo = flightRepo;
            _scheduleRepo = scheduleRepo;
            _mapper = mapper;
            _providers = providers;
        }

        public async Task<FlightDTO> CreateFlightAsync(CreateFlightRequest request)
        {
            var schedule = await _scheduleRepo.GetScheduleByIdAsync(request.ScheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Flight schedule with ID {request.ScheduleId} not found.");

            var flightDate = request.FlightDate.ToDateTime(TimeOnly.MinValue);

            if (flightDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Flight date cannot be in the past.");

            if (flightDate.Date < schedule.EffectiveFrom.Date || flightDate.Date > schedule.EffectiveTo.Date)
                throw new ArgumentException($"Flight date must be between {schedule.EffectiveFrom:yyyy-MM-dd} and {schedule.EffectiveTo:yyyy-MM-dd}.");

            var requestedDay = DateHelper.ConvertDayToFlag(flightDate.DayOfWeek);
            if ((schedule.OperatingDays & requestedDay) == 0)
                throw new ArgumentException($"Schedule does not operate on {flightDate.DayOfWeek}.");

            var existingFlight = await _flightRepo.GetFlightAsync(request.ScheduleId, flightDate);
            if (existingFlight != null)
                throw new InvalidOperationException($"Flight instance already exists for schedule {request.ScheduleId} on {request.FlightDate}.");

            var flight = new Flight
            {
                FlightScheduleId = schedule.Id,
                FlightDate = flightDate,
                EconomyAvailable = schedule.EconomySeats,
                BusinessAvailable = schedule.BusinessSeats,
                FirstClassAvailable = schedule.FirstClassSeats,
                Status = FlightStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdFlight = await _flightRepo.AddFlightAsync(flight);
            createdFlight.FlightSchedule = schedule;

            return BuildFlightDTO(createdFlight);
        }

        public async Task<FlightDTO> GetFlightByIdAsync(int flightId)
        {
            var flight = await _flightRepo.GetFlightByIdAsync(flightId);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with ID {flightId} not found.");

            return BuildFlightDTO(flight);
        }

        public async Task<FlightDTO> GetFlightByScheduleAsync(int scheduleId, DateOnly date)
        {
            var flightDate = date.ToDateTime(TimeOnly.MinValue);

            var existingFlight = await _flightRepo.GetFlightAsync(scheduleId, flightDate);
            if (existingFlight != null)
                return BuildFlightDTO(existingFlight);

            try
            {
                return await CreateFlightAsync(new CreateFlightRequest
                {
                    ScheduleId = scheduleId,
                    FlightDate = date
                });
            }
            catch (InvalidOperationException)
            {
                // Race condition: another request created the flight between our GET and INSERT
                var flight = await _flightRepo.GetFlightAsync(scheduleId, flightDate);
                if (flight == null) throw;
                return BuildFlightDTO(flight);
            }
        }

        private FlightDTO BuildFlightDTO(Flight flight)
        {
            var dto = _mapper.Map<FlightDTO>(flight);
            var schedule = flight.FlightSchedule;

            dto.DepartureDateTime = flight.FlightDate.Date.Add(schedule.DepartureTime.ToTimeSpan());
            dto.ArrivalDateTime = flight.FlightDate.Date.Add(schedule.ArrivalTime.ToTimeSpan());

            if (dto.ArrivalDateTime <= dto.DepartureDateTime)
                dto.ArrivalDateTime = dto.ArrivalDateTime.AddDays(1);

            ApplyPrices(dto, schedule);
            return dto;
        }

        private void ApplyPrices(FlightDTO dto, FlightSchedule schedule)
        {
            var provider = _providers.First(p => p.AirlineCode == dto.AirlineCode);
            dto.EconomyPrice = provider.CalculatePrice(schedule.EconomyBasePrice, CabinClass.Economy);
            dto.BusinessPrice = provider.CalculatePrice(schedule.BusinessBasePrice, CabinClass.Business);
            dto.FirstClassPrice = provider.CalculatePrice(schedule.FirstClassBasePrice, CabinClass.FirstClass);
        }
    }
}
