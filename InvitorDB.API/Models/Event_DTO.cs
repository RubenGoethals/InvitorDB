using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvitorDB.API.Models
{
    public class Event_DTO
    {
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

        //Navigation properties -----------------------
        //many-to-many
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Users { get; set; } = new List<string>();
    }
}
