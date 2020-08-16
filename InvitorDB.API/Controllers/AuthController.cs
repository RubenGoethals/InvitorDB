using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvitorDB.API.Models;
using InvitorDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvitorDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<Person> signInMgr;
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher<Person> hasher;
        private readonly UserManager<Person> userManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(SignInManager<Person> signInMgr, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.AspNetCore.Identity.IPasswordHasher<Person> hasher, UserManager<Person> userManager, ILogger<AuthController> logger)
        {
            this.signInMgr = signInMgr;
            this.configuration = configuration;
            this.hasher = hasher;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpPost]
        [Route("login")]
        //vult de controller basis route aan 
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            //LoginViewModel met (Required) UserName en Password aanbrengen. 
            var returnMessage = "";
            if (!ModelState.IsValid) return BadRequest("Onvolledige gegevens.");
            try
            {
                //geen persistence, geen lockout -> via false, false 
                var result = await signInMgr.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok("Welkom " + loginDTO.UserName);
                }
                throw new Exception("User of paswoord niet gevonden.");
                //zo algemeen mogelijk response. Vertelt niet dat het pwd niet juist is. 
            } catch (Exception exc)
            {
                returnMessage = $"Foutief of ongeldig request: {exc.Message}";
                ModelState.AddModelError("", returnMessage);
            }
            return BadRequest(returnMessage); //zo weinig mogelijk (hacker) info 
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateJwtToken([FromBody] LoginDTO identityDTO)
        {
            try
            {
                var jwtsvc = new JWTServices<Person>(configuration, logger, userManager, hasher);
                var token = await jwtsvc.GenerateJwtToken(identityDTO);
                return Ok(token);
            }
            catch (Exception exc)
            {
                logger.LogError($"Exception thrown when creating JWT: {exc}");
            }
            //Bij niet succesvolle authenticatie wordt een Badrequest (=zo weinig mogelijke info) teruggeven. 
            return BadRequest("Failed to generate JWT token");
        }
    }
}
