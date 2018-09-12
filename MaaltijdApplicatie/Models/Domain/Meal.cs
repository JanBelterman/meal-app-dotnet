using System;
using System.Collections.Generic;

namespace MaaltijdApplicatie.Models.Domain {

    public class Meal {

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public virtual AppUser StudentCook { get; set; }

        public int MaxGuests { get; set; }

        public virtual ICollection<MealStudent> StudentsGuests { get; set; }

    }

}
