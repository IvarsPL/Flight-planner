using FlightPlannerWeb.Models;
using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlannerWeb.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
           var flight = FlightStorage.GetById(id);
           if (flight == null) return NotFound();
            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(Flight flight)
        {
            if (!FlightStorage.IsValid(flight)) return BadRequest(); //400
            if (FlightStorage.Exists(flight)) return Conflict(); //409
            FlightStorage.AddFlight(flight);
            return Created("", flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var flight = FlightStorage.GetById(id);
            FlightStorage.DeleteFlight(flight);
            return Ok();
        }

    }
}
