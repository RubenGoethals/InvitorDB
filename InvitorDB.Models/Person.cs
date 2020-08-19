using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvitorDB.Models
{
    public class Person : IdentityUser
    {
        public enum GenderType
        {
            [Display(Name = "Mannelijk")]
            Male = 0,
            [Display(Name = "Vrouwelijk")]
            Female = 1
        }

        [Display(Name = "Geboorte datum")]
        [DataType(DataType.DateTime, ErrorMessage = "Kies een datum")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Geslacht")]
        //[Required(ErrorMessage = "Verplichte keuze.")]
        [EnumDataType(typeof(GenderType), ErrorMessage = "{0} is geen geldige keuze.")]
        [Range(0, 1, ErrorMessage = "ongeldige keuze")]
        public GenderType Gender { get; set; }

        [ScaffoldColumn(false)]
        public int Bonus { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string ImgUrl
        {
            get
            {
                return (this.UserName != null) ? "/Images/" + UserName.Trim() + ".jpg" : "";
            }
        }

        public ICollection<EvaluationForms> EvaluationForms { get; set; }

        public ICollection<PersonsEvents> PersonsEvents { get; set; }

        public ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();
    }
}
