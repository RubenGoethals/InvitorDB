using System;

namespace InvitorDB.Models
{
    public class PersonsEvents
    {
        public string PersonId { get; set; }
        public int? EventId { get; set; }
        public DateTime DateOfEntry { get; set; } = DateTime.Now;
        public bool Reserve { get; set; }

        //navigatie properties - many to many
        public Person Person { get; set; }
        public Event Event { get; set; }
    }
}
