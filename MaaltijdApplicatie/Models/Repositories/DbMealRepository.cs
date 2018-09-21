using System;
using System.Linq;
using MaaltijdApplicatie.Models.Context;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MaaltijdApplicatie.Models.Repositories {

    public class DbMealRepository : IMealRepository {

        private AppDomainDbContext database;

        public DbMealRepository(AppDomainDbContext database) {
            this.database = database;
        }

        public IQueryable<Meal> GetMeals() =>
            database.Meals.Include(meal => meal.Cook).Include(meal => meal.Guests);

        public Meal GetMeal(int id) =>
            database.Meals.Include(meal => meal.Cook).Include(meal => meal.Guests).ThenInclude(mStud => mStud.Student).FirstOrDefault(m => m.Id == id);

        public IQueryable<Meal> GetMeals(DateTime startDate, DateTime endDate) =>
            database.Meals.Include(meal => meal.Cook).Include(meal => meal.Guests).ThenInclude(mStud => mStud.Student).Where(m => m.DateTime.Date >= startDate.Date && m.DateTime.Date <= endDate.Date);

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

        // Adds a student to the guests of a meal
        public void JoinMeal(Meal meal, Student student) {

            // Add student to guests
            database.Guests.Add(new Guest() { MealId = meal.Id, StudentId = student.Id });
            // Save changes
            database.SaveChanges();

        }

        // Removes the student from the guests of a meal
        public void LeaveMeal(int mealId, int studentId) {

            // Get guest record
            Guest guest = database.Guests.FirstOrDefault(g => g.MealId == mealId && g.StudentId == studentId);
            // If guest exists -> remove it
            if (guest != null) {
                database.Guests.Remove(guest);
            }
            // Save changes
            database.SaveChanges();

        }

    }

}
