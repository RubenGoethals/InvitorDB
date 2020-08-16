using System.ComponentModel.DataAnnotations;

namespace InvitorDB.Webapp.ViewModels
{
    public class DeleteRole_VM
    {
        [Display(Name = "Role name")]
        public string RoleName { get; set; }
    }
}
