using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlannerWeb.Models
{
    public class FlightSearch
    {
        public string from { get; set; }
        public string to { get; set; }
        public string departureDate { get; set; }
    }
}
