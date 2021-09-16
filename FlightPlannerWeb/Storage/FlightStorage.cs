using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
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

        public static void DeleteFlight(Flight flight)
        {
            _flights.Remove(flight);
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
            if (
                flight.From != null
                && flight.To != null
                && !string.IsNullOrEmpty(flight.From.City)
                && !string.IsNullOrEmpty(flight.From.Country)
                && !string.IsNullOrEmpty(flight.From.airport)
                && !string.IsNullOrEmpty(flight.To.City)
                && !string.IsNullOrEmpty(flight.To.Country)
                && !string.IsNullOrEmpty(flight.To.airport)
                && !string.IsNullOrEmpty(flight.Carrier)
                && flight.ArrivalTime != null
                && flight.DepartureTime != null
                && !String.Equals(Regex.Replace(flight.From.airport, @"\s", ""), Regex.Replace(flight.To.airport, @"\s", ""), StringComparison.CurrentCultureIgnoreCase)
                && DateTime.Compare(DateTime.Parse(flight.DepartureTime, CultureInfo.InvariantCulture), DateTime.Parse(flight.ArrivalTime, CultureInfo.InvariantCulture)) < 0
                )
            {
                return true;
            }

            return false;
        }
    }
}
