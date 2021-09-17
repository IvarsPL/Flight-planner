﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlightPlannerWeb.Models;
using FlightPlannerWeb.Storage;
using Microsoft.AspNetCore.Authorization;

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
            if (fs.From == fs.To) return BadRequest();
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