using InvitorDB.Models;
using InvitorDB.Webapp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvitorDB.Webapp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Person> userManager;
        private readonly RoleManager<Role> roleManager;

        public AdminController(UserManager<Person> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: AdminController
        public ActionResult IndexUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        public ActionResult IndexRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        // GET: AdminController/AddRoleToUser
        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(string id)
        {
            Person user = new Person();

            //Op basis van het userId halt de _userManager de volledige user op
            if (!String.IsNullOrEmpty(id))
            {
                user = await userManager.FindByIdAsync(id);
            }
            if (user == null) return RedirectToAction("IndexUsers", roleManager.Roles);

            //Reeds toegekende rollen 
            var assignRolesToUserVM = new RolesForUser_VM()
            {
                AssignedRoles = await userManager.GetRolesAsync(user),
                UnAssignedRoles = new List<string>(),
                User = user,
                UserId = id
            };

            //Nog niet toegekende rollen 
            foreach (var identityRole in roleManager.Roles)
            {
                if (!await userManager.IsInRoleAsync(user, identityRole.Name))
                {
                    assignRolesToUserVM.UnAssignedRoles.Add(identityRole.Name);
                }
            }
            return View(assignRolesToUserVM);
        }

        // POST: AdminController/AddRoleToUser
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(string id, RolesForUser_VM rolesForUser_VM)
        {
            var user = await userManager.FindByIdAsync(id);
            var role = await roleManager.FindByNameAsync(rolesForUser_VM.RoleId);
            var result = await userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("AddRoleToUser", roleManager.Roles);
            }
            return View(rolesForUser_VM);
        }

        // GET: AdminController/RemoveRoleFromUser
        [HttpGet]
        public async Task<IActionResult> RemoveRoleFromUser(string id)
        {
            Person user = new Person();

            //Op basis van het userId halt de _userManager de volledige user op
            if (!String.IsNullOrEmpty(id))
            {
                user = await userManager.FindByIdAsync(id);
            }
            if (user == null) return RedirectToAction("IndexUsers", roleManager.Roles);

            //Reeds toegekende rollen 
            var assignRolesToUserVM = new RolesForUser_VM()
            {
                AssignedRoles = await userManager.GetRolesAsync(user),
                UnAssignedRoles = new List<string>(),
                User = user,
                UserId = id
            };

            return View(assignRolesToUserVM);
        }

        // POST: AdminController/RemoveRoleFromUser
        [HttpPost]
        public async Task<IActionResult> RemoveRoleFromUser(string id, RolesForUser_VM rolesForUser_VM)
        {
            var user = await userManager.FindByIdAsync(id);
            var role = await roleManager.FindByNameAsync(rolesForUser_VM.RoleId);
            var result = await userManager.RemoveFromRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("RemoveRoleFromUser", roleManager.Roles);
            }
            return View(rolesForUser_VM);
        }

        // GET: AdminController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: AdminController/CreateRole
        [HttpGet]
        public IActionResult CreateRole() => View();

        // POST: AdminController/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(AddRole_VM addRoleVM)
        {
            if (ModelState.IsValid)
            {
                var role = new Role()
                {
                    Name = addRoleVM.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRoles", roleManager.Roles);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(addRoleVM);
        }

        // GET: AdminController/EditRole
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            if (id == null)
            {
                //return BadRequest();  //model nog null
                return Redirect("/Error/400");
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                //return BadRequest();  //ADO
                ModelState.AddModelError("", "Not Found.");
            }

            var editRoleVM = new EditRole_VM()
            {
                RoleName = role.Name,
                Users = await userManager.GetUsersInRoleAsync(role.Name)
            };

            return View(editRoleVM);
        }

        // POST: AdminController/EditRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, EditRole_VM editRole_VM)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(id);
                role.Name = editRole_VM.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRoles", roleManager.Roles);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(editRole_VM);
        }

        // GET: AdminController/DeleteRole
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null) { return BadRequest(); }
            //return View ("Error", new ErrorViewModel { RequestId =  HttpContext.TraceIdentifier });

            var role = await roleManager.FindByIdAsync(id);
            if (role == null) { ModelState.AddModelError("", "Not Found."); }
            return View(role);
        }

        // POST: AdminController/DeleteRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null) { throw new Exception(" Bad Delete Request"); }
                    // TODO: Add delete logic here
                    await roleManager.DeleteAsync(await roleManager.FindByIdAsync(id));
                    //if (deleted is null)
                    //    throw new Exception(" Invalid dataentry.");
                }
                return RedirectToAction(nameof(IndexRoles));
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Delete actie mislukt." + exc.Message);

                return View();
            }
        }

        // GET: AdminController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                //return BadRequest();  //model nog null
                return Redirect("/Error/400");
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                //return BadRequest();  //ADO
                ModelState.AddModelError("", "Not Found.");
            }
            return View(user);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IFormCollection collection, Person person)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var result = await userManager.UpdateAsync(person);
                if (result == null)
                {
                    //throw new Exception(" Not Found.");
                    return Redirect("/Error/400");
                }

                return RedirectToAction(nameof(IndexUsers));
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Update actie mislukt." + exc.Message);

                return View(person);
            }
        }

        // GET: AdminController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            //return View ("Error", new ErrorViewModel { RequestId =  HttpContext.TraceIdentifier });

            var user = await userManager.FindByIdAsync(id);
            if (user == null) { ModelState.AddModelError("", "Not Found."); }
            return View(user);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null) { throw new Exception(" Bad Delete Request"); }
                    // TODO: Add delete logic here
                    await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
                    //if (deleted is null)
                    //    throw new Exception(" Invalid dataentry.");
                }
                return RedirectToAction(nameof(IndexUsers));
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Delete actie mislukt." + exc.Message);

                return View();
            }
        }
    }
}
