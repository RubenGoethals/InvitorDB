using InvitorDB.Models;
using InvitorDB.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvitorDB.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {

        private readonly IEventRepo eventRepo;
        public EventController(IEventRepo eventRepo)
        {
            this.eventRepo = eventRepo;
        }

        [HttpGet]
        [Route("GetEvents")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEvents()
        {
            var returnMessage = "";
            try
            {
                var ev = await eventRepo.GetAllEventsAsync();
                if (ev == null)
                {
                    return NotFound();
                }
                return Ok(ev);
            }
            catch (Exception ex)
            {
                returnMessage = $"Foutief of ongeldig request: {ex.Message}";
            }
            return BadRequest(returnMessage); 
        }

        [HttpPost]
        [Route("AddEvent")]
        [AllowAnonymous]
        public async Task<IActionResult> AddEvent([FromBody] Event model)
        {
            var returnMessage = "";
            try
            {
                var ev = await eventRepo.Add(model);
                if (ev == null)
                {
                    return NotFound();
                }
                return Ok(ev);
            }
            catch (Exception ex)
            {
                returnMessage = $"Foutief of ongeldig request: {ex.Message}";
            }
            return BadRequest(returnMessage);
        }

        [HttpPost]
        [Route("DeleteEvent")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEvent(int? id)
        {
            if (id == null) return BadRequest();
            var returnMessage = "";

            try
            {
                await eventRepo.Delete(id.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                returnMessage = $"Foutief of ongeldig request: {ex.Message}";
            }
            return BadRequest(returnMessage);
        }

        [HttpPut]
        [Route("UpdateEvent")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromBody] Event model)
        {
            var returnMessage = "";
            try
            {
                var ev = await eventRepo.Update(model);
                if (ev == null)
                {
                    return NotFound();
                }
                return Ok(ev);
            }
            catch (Exception ex)
            {
                returnMessage = $"Foutief of ongeldig request: {ex.Message}";
            }
            return BadRequest(returnMessage);
        }
    }
}