using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InvitorDB.Models.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private readonly UserManager<Person> userMgr;
        private readonly RoleManager<Role> roleManager;

        public PersonRepo(UserManager<Person> userMgr, RoleManager<Role> roleManager)
        {
            this.userMgr = userMgr;
            this.roleManager = roleManager;
        }

        public async Task<Person> Add(Person person)
        {
            IdentityResult result = await userMgr.CreateAsync(person);
            return null;
        }

        public Task PersonsToEvent(string id, string[] selectedEventsString)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var result = userMgr.Users.Include(u => u.PersonRoles).Where(p => p.PersonRoles.Any(r => r.RoleId == role.Id)).Include(p => p.PersonsEvents);

            //  IList<Person> result = await userMgr.GetUsersInRoleAsync(roleName) ;
            return result;
        }

        public Task<IEnumerable<Person>> GetByEventAsync(int eventId, string role)
        {
            throw new NotImplementedException();
        }


        public async Task<Person> GetForIdAsync(string id)
        {
            return await userMgr.FindByIdAsync(id);
        }

        public bool PersonExists(string id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveEvents(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Person> Update(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
