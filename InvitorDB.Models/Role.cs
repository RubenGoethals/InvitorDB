using InvitorDB.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvitorDB.Models
{
    public class Role : IdentityRole<string>
    {
        public Role() : base() { this.Id = Guid.NewGuid().ToString(); }
        public Role(string name) : base(name) { this.Id = Guid.NewGuid().ToString(); } //nodig in Seeder 
        public string Description { get; set; }
        //public string EventId { get; set; } //navigatie properties 
        //public virtual Event ev { get; set; }
        public ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();

    }
}
