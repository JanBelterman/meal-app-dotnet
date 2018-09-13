using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.Domain {

    public class Meal {

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual AppUser StudentCook { get; set; }

        [Required]
        public int MaxGuests { get; set; }

        public virtual ICollection<MealStudent> StudentsGuests { get; set; }

    }

}
