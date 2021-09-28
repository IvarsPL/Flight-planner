using FlightPlannerWeb.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlannerWeb.Models
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;

        public TestingController(FlightPlannerDbContext context)
        {
            _context = context;
        }
        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            foreach (var i in _context.Airports)
            {
                _context.Airports.Remove(i);
            }

            foreach (var i in _context.Flights)
            {
                _context.Flights.Remove(i);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
