using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvitorDB.Models
{
    public class Event
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Locatie is verplicht")]
        [Display(Name = "Locatie")]
        [MaxLength(512)]
        public string Location { get; set; }

        [Display(Name = "Datum event")]
        [DataType(DataType.DateTime, ErrorMessage = "Kies een datum")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? DateOfEvent { get; set; }

        [Display(Name = "Datum einde registratie")]
        [DataType(DataType.DateTime, ErrorMessage = "Kies een datum")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? EndOfRegistration { get; set; }

        [MinLength(0)]
        [MaxLength(1024)]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Maximum aantal personen")]
        public int MaxPersons { get; set; }

        public ICollection<PersonsEvents> PersonsEvents { get; set; }

        public ICollection<EvaluationForms> EvaluationForms { get; set; }

        //public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
