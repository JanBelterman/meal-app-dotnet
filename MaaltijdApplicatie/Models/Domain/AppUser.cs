using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MaaltijdApplicatie.Models.Domain {

    public class AppUser : IdentityUser {

        public string Name { get; set; }
        public virtual ICollection<Meal> CookOfMeals { get; set; } = new List<Meal>();
        public virtual ICollection<StudentGuest> GuestOfMeals { get; set; } = new List<StudentGuest>();


    }

}
