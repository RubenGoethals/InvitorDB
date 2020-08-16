using InvitorDB.Models;
using System.Collections.Generic;

namespace InvitorDB.Webapp.ViewModels
{
    public class RolesForUser_VM
    {
        public Person User { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public IList<string> AssignedRoles { get; set; }
        public IList<string> UnAssignedRoles { get; set; }
    }
}
