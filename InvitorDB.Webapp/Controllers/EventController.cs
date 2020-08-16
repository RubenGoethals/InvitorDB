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
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {

        private readonly IEventRepo eventRepo;
        public EventController(IEventRepo eventRepo)
        {
            this.eventRepo = eventRepo;
        }

        // GET: Event
        public async Task<IActionResult> Index(string eventSearch = null, string sortField = "Name")
        {

            ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.search = eventSearch;

            var events = await eventRepo.GetAllEventsAsync(eventSearch, sortField);

            if (events is null)
            {
                return Redirect("/Error/400");
            }
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ev = await eventRepo.GetEventForIdAsync(id.Value);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, Event ev)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var created = await eventRepo.Add(ev);
                    if (created is null)
                        throw new Exception(" Invalid dataentry.");
                    return RedirectToAction(nameof(Index));
                }
                return View(ev);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Create is unable to save.");
                ModelState.AddModelError("", "Create Actie mislukt." + exc.Message);
                return View(ev);
            }
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return BadRequest();  //model nog null
                return Redirect("/Error/400");
            }
            var ev = await eventRepo.GetEventForIdAsync(id.Value);
            if (ev == null)
            {
                //return BadRequest();  //ADO
                ModelState.AddModelError("", "Not Found.");
            }
            return View(ev);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, IFormCollection collection, Event ev)
        {
            try
            {
                // TODO: Add update logic here
                if (id == null)
                {
                    return BadRequest();
                }
                var result = await eventRepo.Update(ev);
                if (result == null)
                {
                    //throw new Exception(" Not Found.");
                    return Redirect("/Error/400");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Update actie mislukt." + exc.Message);

                return View(ev);
            }
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            //return View ("Error", new ErrorViewModel { RequestId =  HttpContext.TraceIdentifier });

            var ev = await eventRepo.GetEventForIdAsync(id.Value);
            if (ev == null) { ModelState.AddModelError("", "Not Found."); }
            return View(ev);
        }

        // POST: Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null) { throw new Exception(" Bad Delete Request"); }
                    // TODO: Add delete logic here
                    await eventRepo.Delete(id.Value);
                    //if (deleted is null)
                    //    throw new Exception(" Invalid dataentry.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}