using System;
using System.ComponentModel.DataAnnotations;

namespace InvitorDB.Models
{
    public class EvaluationForms
    {
        public string PersonId { get; set; }
        public int? EventId { get; set; }
        public DateTime DateOfEntry { get; set; } = DateTime.Now;

        [MinLength(0)]
        [MaxLength(1024)]
        [Display(Name = "Beschrijving")]
        [Required]
        public string Comment { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "ongeldige keuze")]
        public int Review { get; set; }

        //navigatie properties - many to many
        public Person Person { get; set; }
        public Event Event { get; set; }
    }
}
