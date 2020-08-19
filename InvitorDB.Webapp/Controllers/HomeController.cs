using InvitorDB.Models;
using InvitorDB.Models.Repositories;
using InvitorDB.Webapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InvitorDB.Webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventRepo eventRepo;
        private readonly UserManager<Person> userManager;

        public HomeController(ILogger<HomeController> logger, IEventRepo eventRepo, UserManager<Person> userManager)
        {
            _logger = logger;
            this.eventRepo = eventRepo;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string eventSearch = null)
        {
            ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.search = eventSearch;
     
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsLoggedIn = true;
            }
            else
            {
                ViewBag.IsLoggedIn = false;
            }

            var events = await eventRepo.GetAllEventsAsync(eventSearch);

            if (events is null)
            {
                return Redirect("/Error/400");
            }
            return View(events);

        }

        public async Task<IActionResult> IndexUsers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ev = await eventRepo.GetPersonInEvent(id.Value);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }


        public async Task<IActionResult> IndexEvaluations(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ev = await eventRepo.GetEvaluationsForIdAsync(id.Value);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
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

        // GET: HomeController/AddPersonToEvent
        public async Task<IActionResult> AddPersonToEvent(int? id)
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

        // POST: HomeController/AddPersonToEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPersonToEvent(int? id, IFormCollection collection)
        {
            PersonsEvents created = null;
            var ev = await eventRepo.GetEventForIdAsync(id.Value);
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(HttpContext.User);
                    var amount = await eventRepo.GetAmountPersons(ev.Id);
                    if (amount <= ev.MaxPersons)
                    {
                        created = await eventRepo.AddEventToPerson(user.Id, ev.Id, false);
                    }
                    else if (amount <= ev.MaxPersons + 3)
                    {
                        created = await eventRepo.AddEventToPerson(user.Id, ev.Id, true);
                    }
                    
                    if (created is null)
                        throw new Exception(" Invalid dataentry.");

                    user.Bonus += 5;
                    var result = await userManager.UpdateAsync(user);
                    if (result == null)
                    {
                        return Redirect("/Error/400");
                    }

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                Console.WriteLine("Create is unable to save.");
                ModelState.AddModelError("", "Create Actie mislukt." + exc.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: HomeController/AddEvalutionToEvent
        public ActionResult AddEvaluationToEvent(int? id) => View();

        // POST: HomeController/AddEvalutionToEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvaluationToEvent(int? id, IFormCollection collection, EvaluationForms evaluationForms)
        {
            EvaluationForms created = null;
            var ev = await eventRepo.GetEventForIdAsync(id.Value);
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(HttpContext.User);
                    evaluationForms.EventId = ev.Id;
                    evaluationForms.PersonId = user.Id;
                    created = await eventRepo.AddEvalutionToEvent(evaluationForms);
                    
                    if (created is null)
                        throw new Exception(" Invalid dataentry.");
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                Console.WriteLine("Create is unable to save.");
                ModelState.AddModelError("", "Create Actie mislukt." + exc.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
