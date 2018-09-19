using System;
using System.Collections;
using System.Linq;
using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    public interface IMealRepository {

        IQueryable<Meal> GetMeals();

        Meal GetMeal(int id);

        IQueryable<Meal> GetMeals(DateTime startDate, DateTime endDate);

        void SaveMeal(Meal meal);

        void RegisterForMeal(Meal meal, AppUser student);
        void UnsubscribeFromMeal(int mealId, string studentId);

    }

}
