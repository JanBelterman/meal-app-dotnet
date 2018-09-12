using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MaaltijdApplicatie.Models.Domain {

    public class AppUser : IdentityUser {

        public string Name { get; set; }
        public virtual ICollection<Meal> CookOfMeals { get; set; }
        public virtual ICollection<MealStudent> GuestOfMeals { get; set; }


    }

}
