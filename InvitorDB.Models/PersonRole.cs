using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvitorDB.Models
{
    public class PersonRole : IdentityUserRole<string>
    {
        public DateTime DateOfEntry { get; set; } = DateTime.Now;
            //geen UserId noch RoleId (resulteert in dubbele properties) 
            //navigatie properties 
        public virtual Role Role { get; set; } 
        public virtual Person User { get; set; }
    }
}
