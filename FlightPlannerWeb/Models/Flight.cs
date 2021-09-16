using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlannerWeb.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public Airport To { get; set; }
        public Airport From { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public override bool Equals(object flight)
        {
           var item = (Flight) flight;
            
            if (item == null)
            {
                return false;
            }

            return this.From.City == item.From.City
                   && this.From.Country == item.From.Country
                   && this.From.airport == item.From.airport
                   && this.To.City == item.To.City
                   && this.To.Country == item.To.Country
                   && this.To.airport == item.To.airport
                   && this.Carrier == item.Carrier
                   && this.ArrivalTime == item.ArrivalTime
                   && this.DepartureTime == item.DepartureTime;
        }
    }
}
