using FlightPlannerWeb.DbContext;
using FlightPlannerWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlightPlannerWeb.Storage
{
    public static class FlightStorage
    {
        private static object _balanceLock = new();
        public static Flight FindFlight(int id, FlightPlannerDbContext context)
        {
            return context.Flights
                .Include(a => a.To)
                .Include(a => a.From)
                .SingleOrDefault(f => f.Id == id);
        }
        
        public static Flight GetById(int id, FlightPlannerDbContext context)
        {
            return FindFlight(id, context);
        }

        public static Airport[] GetAirport(string search, FlightPlannerDbContext context)
        {
            var airports = new Airport[1];

            search = Regex.Replace(search, @"\s", "").ToLower();
            foreach (var flight in context.Airports)
            {
                if (flight.City.ToLower().Contains(search)) airports[0] = flight;
                if (flight.Country.ToLower().Contains(search)) airports[0] = flight;
                if (flight.airport.ToLower().Contains(search)) airports[0] = flight;
                if (flight.City.ToLower().Contains(search)) airports[0] = flight;
                if (flight.Country.ToLower().Contains(search)) airports[0] = flight;
                if (flight.airport.ToLower().Contains(search)) airports[0] = flight;
            }

            return airports;
        }

        public static bool Exists(Flight flight, FlightPlannerDbContext context)
        {
            Flight result = context.Flights
                .Include(a => a.To)
                .Include(a => a.From)
                .FirstOrDefault(f => f.ArrivalTime == flight.ArrivalTime
                                     && f.Carrier == flight.Carrier
                                     && f.DepartureTime == flight.DepartureTime
                                     && f.From.airport == flight.From.airport
                                     && f.From.City == flight.From.City
                                     && f.From.Country == flight.From.Country
                                     && f.To.airport == flight.To.airport
                                     && f.To.City == flight.To.City
                                     && f.To.Country == flight.To.Country);

            return result != null;
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

        public static PageResult SearchFlight(FlightSearch fs, FlightPlannerDbContext context)
        {
            lock (_balanceLock)
            {
                Flight[] searchedFlights = context.Flights
                    .Include(a => a.To)
                    .Include(a => a.From)
                    .Where(fl =>
                        fl.From.airport.ToUpper() == fs.from.Trim().ToUpper()
                        && fl.To.airport.ToUpper() == fs.to.Trim().ToUpper()
                        && fl.DepartureTime.Substring(0, 10) == fs.departureDate).ToArray();

                PageResult result = new PageResult(searchedFlights);

                return result;
            }
        }

        public static void DeleteFlight(int id, FlightPlannerDbContext context)
        {
            Flight flight = FindFlight(id, context);

            lock (_balanceLock)
            {
                if (flight != null)
                {
                    context.Airports.Remove(flight.To);
                    context.Airports.Remove(flight.From);
                    context.Flights.Remove(flight);
                    context.SaveChanges();
                }
            }
        }
    }
}
