using System;
using System.Collections.Generic;
using System.Linq;
using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    public class FakeMealRepository : IMealRepository {

        public IQueryable<Meal> GetMeals() {

            return new List<Meal> {
            new Meal { DateTime = new DateTime(2018, 9, 12, 17, 30, 00), Name = "Pizza", Description = "Italiaanse pizza", Price = 4.95M, MaxGuests = 6 },
            new Meal { DateTime = new DateTime(2018, 9, 13, 17, 30, 00), Name = "Spaghetti", Description = "Italiaanse spaghetti", Price = 4.95M, MaxGuests = 6 },
            new Meal { DateTime = new DateTime(2018, 9, 15, 17, 30, 00), Name = "Friet", Description = "Franse friet", Price = 4.95M, MaxGuests = 6 },
            new Meal { DateTime = new DateTime(2018, 9, 18, 17, 30, 00), Name = "Kipschotel", Description = "Sumatraanse kipschotel", Price = 4.95M, MaxGuests = 6 },
            new Meal { DateTime = new DateTime(2018, 9, 22, 17, 30, 00), Name = "Lasagne", Description = "Italiaanse lasagne", Price = 4.95M, MaxGuests = 6 }
        }.AsQueryable<Meal>();

        }

        public void SaveMeal(Meal meal) {



        }

    }

}
