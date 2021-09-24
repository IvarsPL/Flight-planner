using FlightPlannerWeb.Models;
using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlannerWeb.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClientController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirport(string search)
        {
            Airport[] flight = FlightStorage.GetAirport(search);
            return Ok(flight);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(FlightSearch fs)
        {
            if (fs.from == fs.to) return BadRequest();
            PageResult flight = FlightStorage.SearchFlight(fs);
            return Ok(flight);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetById(id);
            if (flight == null) return NotFound();
            return Ok(flight);
        }
    }
}
