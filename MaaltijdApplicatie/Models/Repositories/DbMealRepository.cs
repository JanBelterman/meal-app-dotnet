using System;
using System.Linq;
using MaaltijdApplicatie.Models.Context;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MaaltijdApplicatie.Models.Repositories {

    public class DbMealRepository : IMealRepository {

        // Properties
        private AppDomainDbContext database;
        
        // Consturctor
        public DbMealRepository(AppDomainDbContext database) {
            this.database = database;
        }

        // Get all meals
        public IQueryable<Meal> GetMeals() =>
            database.Meals.Include(meal => meal.Cook).Include(meal => meal.Guests);

        // Gets a single meal by id
        public Meal GetMeal(int id) =>
            database.Meals.Include(meal => meal.Cook).Include(meal => meal.Guests).ThenInclude(mStud => mStud.Student).FirstOrDefault(m => m.Id == id);

        // Get meals between two dates
        public IQueryable<Meal> GetMeals(DateTime startDate, DateTime endDate) =>
            database.Meals.
                Include(meal => meal.Cook).
                Include(meal => meal.Guests).
                ThenInclude(mStud => mStud.Student).
                Where(m => m.DateTime.Date >= startDate.Date && m.DateTime.Date <= endDate.Date);

        // Update or create product
        public void SaveMeal(Meal meal) {

            // Meal does not exist -> create it
            if (meal.Id == 0) {
                database.Meals.Add(meal);
            }
            // Meal does exits -> update it
            else {
                Meal dbEntry = database.Meals.FirstOrDefault(m => m.Id == meal.Id);
                if (dbEntry != null) {
                    dbEntry.Name = meal.Name;
                    dbEntry.Description = meal.Description;
                    dbEntry.Price = meal.Price;
                    dbEntry.MaxGuests = meal.MaxGuests;
                }
            }
            // Save changes
            database.SaveChanges();

        }

        public void Save() =>
            database.SaveChanges();

    }

}
