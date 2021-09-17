using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlannerWeb.Models
{
    public class FlightSearch
    {
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureTime { get; set; }
    }
}
