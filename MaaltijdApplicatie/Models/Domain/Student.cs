using System.Collections.Generic;

namespace MaaltijdApplicatie.Models.Domain {

    public class Student {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public ICollection<Meal> CookOfMeals { get; set; } = new List<Meal>();
        public ICollection<Guest> GuestOfMeals { get; set; } = new List<Guest>();

    }

}
