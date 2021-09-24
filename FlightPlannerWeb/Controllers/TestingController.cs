using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlannerWeb.Models
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights();
            return Ok();
        }
    }
}
