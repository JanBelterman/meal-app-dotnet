using System.Linq;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Users.Models.Context;

namespace MaaltijdApplicatie.Models.Repositories {

    public class DbMealRepository : IMealRepository {

        private AppIdentityDbContext database;

        public DbMealRepository(AppIdentityDbContext database) {
            this.database = database;
        }

        public IQueryable<Meal> GetMeals() {

            return database.Meals.Include(meal => meal.StudentCook).Include(meal => meal.StudentsGuests);

        }

        // Update or create product
        public void SaveMeal(Meal meal) {

            if (meal.Id == 0) {

                database.Meals.Add(meal);

            } else {

                Meal dbEntry = database.Meals.FirstOrDefault(m => m.Id == meal.Id);

                if (dbEntry != null) {
                    dbEntry.Name = meal.Name;
                    // Update other properies too
                }

            }

            database.SaveChanges();

        }

        public void RegisterForMeal(Meal meal, AppUser student) {

            database.MealStudents.Add(new MealStudent() { MealId = meal.Id, AppUserId = student.Id });

            database.SaveChanges();

        }

    }

}
