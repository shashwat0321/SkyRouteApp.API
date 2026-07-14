using AutoMapper;
using SkyRoute.Application.DTOs.Request;
using SkyRoute.Application.DTOs.Response;
using SkyRoute.Application.Interfaces;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IFlightRepository _flightRepo;
        private readonly IEnumerable<IFlightProvider> _providers;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepo,
            IFlightRepository flightRepo,
            IEnumerable<IFlightProvider> providers,
            IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _flightRepo = flightRepo;
            _providers = providers;
            _mapper = mapper;
        }

        public async Task<BookingResponseDTO> CreateBookingAsync(BookingCreateDTO request)
        {
            var flight = await _flightRepo.GetFlightByIdAsync(request.FlightId);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with ID {request.FlightId} not found.");

            if (flight.Status != FlightStatus.Scheduled)
                throw new InvalidOperationException($"Flight is not available for booking. Status: {flight.Status}.");

            var seats = request.Passengers.Count;

            var (available, basePrice) = request.CabinClass switch
            {
                CabinClass.Economy    => (flight.EconomyAvailable,    flight.FlightSchedule.EconomyBasePrice),
                CabinClass.Business   => (flight.BusinessAvailable,   flight.FlightSchedule.BusinessBasePrice),
                CabinClass.FirstClass => (flight.FirstClassAvailable, flight.FlightSchedule.FirstClassBasePrice),
                _ => throw new ArgumentException($"Unknown cabin class: {request.CabinClass}")
            };

            if (seats > available)
                throw new InvalidOperationException(
                    $"Not enough {request.CabinClass} seats. Requested: {seats}, Available: {available}.");

            var provider = _providers.First(p => p.AirlineCode == flight.FlightSchedule.Airline.Code);
            var totalPrice = provider.CalculatePrice(basePrice, request.CabinClass) * seats;

            var booking = new Booking
            {
                UserId        = 1, // hardcoded until JWT is implemented
                FlightId      = request.FlightId,
                CabinClass    = request.CabinClass,
                NumberOfSeats = seats,
                TotalPrice    = totalPrice,
                Status        = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
                Passengers    = _mapper.Map<List<Passenger>>(request.Passengers)
            };

            var created = await _bookingRepo.CreateBookingAsync(booking);

            switch (request.CabinClass)
            {
                case CabinClass.Economy:    flight.EconomyAvailable    -= seats; break;
                case CabinClass.Business:   flight.BusinessAvailable   -= seats; break;
                case CabinClass.FirstClass: flight.FirstClassAvailable -= seats; break;
            }
            await _bookingRepo.SaveChangesAsync();

            return _mapper.Map<BookingResponseDTO>(created);
        }

        public async Task<BookingResponseDTO> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            return _mapper.Map<BookingResponseDTO>(booking);
        }

        public async Task<IEnumerable<BookingResponseDTO>> GetBookingsByUserIdAsync(int userId)
        {
            var bookings = await _bookingRepo.GetBookingsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
        }

        public async Task<BookingResponseDTO> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            if (booking.Flight.Status == FlightStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a booking for a flight that has already completed.");

            switch (booking.CabinClass)
            {
                case CabinClass.Economy:    booking.Flight.EconomyAvailable    += booking.NumberOfSeats; break;
                case CabinClass.Business:   booking.Flight.BusinessAvailable   += booking.NumberOfSeats; break;
                case CabinClass.FirstClass: booking.Flight.FirstClassAvailable += booking.NumberOfSeats; break;
            }

            booking.Status = BookingStatus.Cancelled;
            if (booking.PaymentStatus == PaymentStatus.Paid)
                booking.PaymentStatus = PaymentStatus.Refunded;

            await _bookingRepo.SaveChangesAsync();
            return _mapper.Map<BookingResponseDTO>(booking);
        }
    }
}
