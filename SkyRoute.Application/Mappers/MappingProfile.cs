using AutoMapper;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FlightSchedule, ScheduleSearchResultDTO>()
               .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightNumber))
               .ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.Airline.Name))
               .ForMember(dest => dest.AirlineCode, opt => opt.MapFrom(src => src.Airline.Code))
               .ForMember(dest => dest.DepartureAirportCode, opt => opt.MapFrom(src => src.DepartureAirport.Code))
               .ForMember(dest => dest.DepartureCity, opt => opt.MapFrom(src => src.DepartureAirport.City))
               .ForMember(dest => dest.ArrivalAirportCode, opt => opt.MapFrom(src => src.ArrivalAirport.Code))
               .ForMember(dest => dest.ArrivalCity, opt => opt.MapFrom(src => src.ArrivalAirport.City))
               .ForMember(dest => dest.DepartureDateTime, opt => opt.MapFrom(src => DateTime.MinValue.Add(src.DepartureTime.ToTimeSpan())))
               .ForMember(dest => dest.ArrivalDateTime, opt => opt.MapFrom(src => DateTime.MinValue.Add(src.ArrivalTime.ToTimeSpan())))
               .ForMember(dest => dest.EconomyPrice, opt => opt.Ignore());

            CreateMap<Flight, FlightDTO>()
               .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.FlightScheduleId))
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightSchedule.FlightNumber))
               .ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.FlightSchedule.Airline.Name))
               .ForMember(dest => dest.AirlineCode, opt => opt.MapFrom(src => src.FlightSchedule.Airline.Code))
               .ForMember(dest => dest.DepartureAirportCode, opt => opt.MapFrom(src => src.FlightSchedule.DepartureAirport.Code))
               .ForMember(dest => dest.DepartureCity, opt => opt.MapFrom(src => src.FlightSchedule.DepartureAirport.City))
               .ForMember(dest => dest.ArrivalAirportCode, opt => opt.MapFrom(src => src.FlightSchedule.ArrivalAirport.Code))
               .ForMember(dest => dest.ArrivalCity, opt => opt.MapFrom(src => src.FlightSchedule.ArrivalAirport.City))
               .ForMember(dest => dest.EconomyAvailable, opt => opt.MapFrom(src => src.EconomyAvailable))
               .ForMember(dest => dest.BusinessAvailable, opt => opt.MapFrom(src => src.BusinessAvailable))
               .ForMember(dest => dest.FirstClassAvailable, opt => opt.MapFrom(src => src.FirstClassAvailable))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.DepartureDateTime, opt => opt.Ignore())
               .ForMember(dest => dest.ArrivalDateTime, opt => opt.Ignore())
               .ForMember(dest => dest.EconomyPrice, opt => opt.Ignore())
               .ForMember(dest => dest.BusinessPrice, opt => opt.Ignore())
               .ForMember(dest => dest.FirstClassPrice, opt => opt.Ignore());

            CreateMap<Airport, AirportResponse>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

            CreateMap<PassengerCreateDTO, Passenger>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.Booking, opt => opt.Ignore());

            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
