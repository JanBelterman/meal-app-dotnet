using System;
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

        public IQueryable<Meal> GetMeals() =>
            database.Meals.Include(meal => meal.StudentCook).Include(meal => meal.StudentsGuests);

        public Meal GetMeal(int id) =>
            database.Meals.Include(meal => meal.StudentCook).Include(meal => meal.StudentsGuests).ThenInclude(mStud => mStud.AppUser).FirstOrDefault(m => m.Id == id);

        public IQueryable<Meal> GetMeals(DateTime startDate, DateTime endDate) =>
            database.Meals.Include(meal => meal.StudentCook).Include(meal => meal.StudentsGuests).ThenInclude(mStud => mStud.AppUser).Where(m => m.DateTime.Date >= startDate.Date && m.DateTime.Date <= endDate.Date);

        // Update or create product
        public void SaveMeal(Meal meal) {

            if (meal.Id == 0) {

                database.Meals.Add(meal);

            } else {

                Meal dbEntry = database.Meals.FirstOrDefault(m => m.Id == meal.Id);

                if (dbEntry != null) {
                    dbEntry.Name = meal.Name;
                    dbEntry.Description = meal.Description;
                    dbEntry.Price = meal.Price;
                    dbEntry.MaxGuests = meal.MaxGuests;
                }

            }

            database.SaveChanges();

        }

        public void RegisterForMeal(Meal meal, AppUser student) {

            database.MealStudents.Add(new MealStudent() { MealId = meal.Id, AppUserId = student.Id });

            database.SaveChanges();

        }

        public void UnsubscribeFromMeal(int mealId, string studentId) {

            var mealStudent = database.MealStudents.FirstOrDefault(m => m.MealId == mealId && m.AppUserId == studentId);
            if (mealStudent != null) {
                database.MealStudents.Remove(mealStudent);
            }

            database.SaveChanges();

        }

    }

}
