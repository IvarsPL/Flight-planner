using FlightPlannerWeb.DbContext;
using FlightPlannerWeb.Models;
using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlannerWeb.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClientController : ControllerBase
    {
        private static object _balanceLock = new();
        private readonly FlightPlannerDbContext _context;
        public CustomerClientController(FlightPlannerDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirport(string search)
        {
            Airport[] flight = FlightStorage.GetAirport(search, _context);
            return Ok(flight);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(FlightSearch fs)
        {
            if (fs.from == fs.to) return BadRequest();
            PageResult flight = FlightStorage.SearchFlight(fs, _context);
            return Ok(flight);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetById(id, _context);
            if (flight == null) return NotFound();
            return Ok(flight);
        }
    }
}
