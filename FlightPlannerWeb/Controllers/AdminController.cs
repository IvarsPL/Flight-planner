using FlightPlannerWeb.DbContext;
using FlightPlannerWeb.Models;
using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlannerWeb.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;
        public AdminController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _context.Flights
                .Include(a => a.To)
                .Include(a => a.From)
                .SingleOrDefaultAsync(f => f.Id == id);
            //FlightStorage.GetById(id);
            if (flight == null) return NotFound();
            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(Flight flight)
        {
            if (!FlightStorage.IsValid(flight)) return BadRequest(); //400
            if (FlightStorage.Exists(flight)) return Conflict(); //409
            _context.Flights.Add(flight);
            _context.SaveChanges();
            //FlightStorage.AddFlight(flight);
            return Created("", flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            FlightStorage.DeleteFlight(id);
            return Ok();
        }

    }
}
