using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using FlightPlannerWeb.Models;

namespace FlightPlannerWeb.Storage
{
    public static class FlightStorage
    {
        private static readonly List<Flight> _flights = new List<Flight>();
        private static int _flightId = 1;

        public static Flight GetById(int id)
        {
            return _flights.SingleOrDefault(f => f.Id == id);
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = _flightId;
            _flights.Add(flight);
            _flightId++;
            return flight;

        }

        public static bool Exists(Flight flight)
        {
            foreach (var f in _flights)
            {
                if (f.Equals(flight)) return true;
            }
            
            return false;
        }

        public static bool IsValid(Flight flight)
        {
            if (flight.Id > 0)
            {
                return true;
            }

            return false;
        }
    }
}
