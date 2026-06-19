using AutoMapper;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappers
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Flight, FlightResponseDTO>()
                .ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.Airline.Name))
                .ForMember(dest => dest.AirlineCode, opt => opt.MapFrom(src => src.Airline.Code))
                .ForMember(dest => dest.DepartureAirportCode, opt => opt.MapFrom(src => src.DepartureAirport.Code))
                .ForMember(dest => dest.DepartureCity, opt => opt.MapFrom(src => src.DepartureAirport.City))
                .ForMember(dest => dest.ArrivalAirportCode, opt => opt.MapFrom(src => src.ArrivalAirport.Code))
                .ForMember(dest => dest.ArrivalCity, opt => opt.MapFrom(src => src.ArrivalAirport.City))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}