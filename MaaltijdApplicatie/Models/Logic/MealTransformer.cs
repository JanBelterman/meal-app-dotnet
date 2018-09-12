using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaaltijdApplicatie.Models.Logic {

    // Transforms meals, so that all dates are added to the list for the coming 2 weeks and dates are filled with available meals
    public class MealTransformer {

        public static IEnumerable<MealDate> TransformMeals(IEnumerable<Meal> meals) {

            // Create list
            List<MealDate> mealDates = new List<MealDate>();

            // Get all dates for coming 2 weeks
            // Insert meals in correct dates
            foreach (var d in DateTime.Now.GetDatesForComingTwoWeeks()) {

                var mealDate = new MealDate() {
                    Date = d,
                    Meal = meals.FirstOrDefault(m => m.DateTime.Date == d.Date) // Insert meal or set null
                };

                mealDates.Add(mealDate);

            }

            // Return list
            return mealDates;

        }

    }

}
