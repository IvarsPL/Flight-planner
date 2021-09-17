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
        private static object _balanceLock = new ();

        public static Flight GetById(int id)
        {
            lock (_balanceLock)
            {
                return _flights.SingleOrDefault(f => f.Id == id);
            }
           
        }

        public static Airport[] GetAirport(string search)
        {
            var airports = new Airport[1];
            
            search = Regex.Replace(search, @"\s", "").ToLower();
            foreach (var flight in _flights)
            {
                if (flight.From.City.ToLower().Contains(search)) airports[0] = flight.From;
                if (flight.From.Country.ToLower().Contains(search)) airports[0] = flight.From;
                if (flight.From.airport.ToLower().Contains(search)) airports[0] = flight.From;
                if (flight.To.City.ToLower().Contains(search)) airports[0] = flight.To;
                if (flight.To.Country.ToLower().Contains(search)) airports[0] = flight.To;
                if (flight.To.airport.ToLower().Contains(search)) airports[0] = flight.To;
            }

            return airports;
        }

        public static void ClearFlights()
        {
            lock (_balanceLock)
            {
                _flights.Clear();
            }
            
        }

        public static Flight AddFlight(Flight flight)
        {
            lock (_balanceLock)
            {
                flight.Id = _flightId;
                _flights.Add(flight);
                _flightId++;
                return flight;
            }
            

        }

        public static void DeleteFlight(int id)
        {
            lock (_balanceLock)
            {
                var flight = _flights.SingleOrDefault(f => f.Id == id);
                if (flight != null)
                {
                    _flights.Remove(flight);
                }
                
            }
            
        }

        public static bool Exists(Flight flight)
        {
            lock (_balanceLock)
            {
                foreach (var f in _flights)
                {
                    if (f.Equals(flight)) return true;
                }

                return false;
            }
            
        }

        public static bool IsValid(Flight flight)
        {
            lock (_balanceLock)
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

        public static PageResult SearchFlight(FlightSearch fs)
        {
            lock (_balanceLock)
            {
                var flight = _flights.Find(f => f.From.airport == fs.From
                                                && f.To.airport == fs.To
                                                && f.DepartureTime == fs.DepartureTime);
                PageResult result = new PageResult();
                result.Page = 0;
                result.TotalItems = 0;

                if (fs != null)
                {
                    result.Items = new List<Flight>();
                    if (flight != null) result.Items.Add(flight);
                }
                return result;
            }
           
        }
    }
}
