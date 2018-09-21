using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaaltijdApplicatie.Models.Logic {

    // Converts meal into meal dates that contain extra information needed in views
    // The list method also adds days to fill two weeks
    public class MealTransformer {

        // Transforms a list of meals into meal dates
        public static ICollection<MealDate> TransformMeals(IEnumerable<Meal> meals, Student student) {

            // Create model
            ICollection<MealDate> mealDates = new List<MealDate>();

            // Get all dates for coming 2 weeks -> loop through
            // Insert information regarding this meal in the mealdate model
            foreach (var d in DateTime.Now.GetDatesForComingTwoWeeks()) {

                // Get meal for this date
                var meal = meals.FirstOrDefault(m => m.DateTime.Date == d.Date);
                // Store information
                var mealDate = new MealDate() {
                    Date = d,
                    MonthString = d.ToString("MMMM"),
                    DayOfWeekString = UppercaseFirst(d.ToString("dddd")),
                    Meal = meal,
                    UserHasJoined = CheckIfUserIsRegistered(meal, student),
                    UserIsCook = CheckIfUserIsCook(meal, student),
                    MealIsFull = meal?.Guests?.Count >= meal?.MaxGuests && false
                };
                // Add meal date to model
                mealDates.Add(mealDate);

            }

            // Return model
            return mealDates;

        }

        // Transforms a single meal into meal date
        public static MealDate TransformIntoMealDate(Meal meal, Student student = null) {

            // Store information
            MealDate mealDate = new MealDate {
                Date = meal.DateTime,
                Time = meal.DateTime,
                MonthString = meal.DateTime.ToString("MMMM"),
                DayOfWeekString = UppercaseFirst(meal.DateTime.ToString("dddd")),
                Meal = meal,
                UserHasJoined = CheckIfUserIsRegistered(meal, student),
                UserIsCook = CheckIfUserIsCook(meal, student),
                MealIsFull = meal?.Guests?.Count >= meal?.MaxGuests && false
            };

            return mealDate;

        }

        public static void AddDateStrings(MealDate mealDate) {

            mealDate.DayOfWeekString = UppercaseFirst(mealDate.Date.ToString("dddd"));
            mealDate.MonthString = mealDate.Date.ToString("MMMM");

        }

        private static bool CheckIfUserIsRegistered(Meal meal, Student student) { // Change to lambda

            if (meal != null) {

                foreach (Guest guest in meal.Guests) {
                    if (guest?.Student?.Id == student?.Id) {
                        return true;
                    }
                }

                return false;

            }

            return false;

        }

        private static bool CheckIfUserIsCook(Meal meal, Student student) {

            if (meal != null) {

                if (meal?.Cook?.Id == student?.Id) {
                    return true;
                }

            }

            return false;

        }

        private static string UppercaseFirst(string s) {

            // Check for empty string.
            if (string.IsNullOrEmpty(s)) {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);

        }

    }

}
