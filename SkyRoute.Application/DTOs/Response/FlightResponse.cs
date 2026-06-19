namespace SkyRoute.Application.DTOs.Response
{
    public class FlightResponseDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }

        public string AirlineName { get; set; }
        public string AirlineCode { get; set; }

        public string DepartureAirportCode { get; set; }
        public string DepartureCity { get; set; }

        public string ArrivalAirportCode { get; set; }
        public string ArrivalCity { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }
        public decimal Price { get; set; }

        public string Status { get; set; }
    }
}

