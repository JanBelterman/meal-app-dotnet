using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.Domain {

    public class Meal {

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [DisplayName("Naam")]
        [Required(ErrorMessage = "Vul een naam in")]
        public string Name { get; set; }

        [DisplayName("Beschrijving")]
        [Required(ErrorMessage = "Vul een beschrijving in")]
        public string Description { get; set; }

        [DisplayName("Prijs")]
        [Required(ErrorMessage = "Vul een prijs in")]
        public decimal? Price { get; set; }

        [DisplayName("Max. aantal mee eters")]
        [Required(ErrorMessage = "Vul het maximaal aantal mee eters in")]
        public int? MaxGuests { get; set; }

        public virtual AppUser StudentCook { get; set; }

        public virtual ICollection<StudentGuest> StudentsGuests { get; set; } = new List<StudentGuest>();

    }

}
