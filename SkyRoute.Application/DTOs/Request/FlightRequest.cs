using System;
using System.Collections.Generic;
using System.Text;

namespace SkyRoute.Application.DTOs.Request
{
    public class FlightRequestDto
    {
        public string Origin { get; set; }       // "DEL"
        public string Destination { get; set; }   // "BOM"
        public DateTime? Date { get; set; }       // Optional
    }
}
