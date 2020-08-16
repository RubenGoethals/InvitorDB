using InvitorDB.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvitorDB.Webapp.ViewModels
{
    public class EditRole_VM
    {
        [Display(Name = "Role name")]
        public string RoleName { get; set; }
        public IList<Person> Users { get; set; }
    }
}
