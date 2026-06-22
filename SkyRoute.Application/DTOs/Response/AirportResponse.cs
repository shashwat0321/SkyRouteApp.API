using System;
using System.Collections.Generic;
using System.Text;

namespace SkyRoute.Application.DTOs.Response
{
    public  class AirportResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
