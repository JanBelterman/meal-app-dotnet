using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace MaaltijdApplicatie.Components {

    public class WidgetViewComponent : ViewComponent {

        private IMealRepository mealRepository;
        private IStudentRepository studentRepository;

        public WidgetViewComponent(IMealRepository mealRepository, IStudentRepository studentRepository) {
            this.mealRepository = mealRepository;
            this.studentRepository = studentRepository;
        }

        // Create view component
        public IViewComponentResult Invoke() {

            // Create view model
            WidgetViewModel widgetData = new WidgetViewModel();

            // Get meals for coming 2 weeks
            IQueryable<Meal> meals = mealRepository.GetMeals(DateTime.Today, DateTime.Today.AddDays(14));

            // Get TodayInfo
            Meal meal = (Meal) meals.FirstOrDefault(m => m.DateTime.Date == DateTime.Today.Date);
            if (meal != null) {
                widgetData.TodayInfo = "Vandaag eten er " + (meal.Guests.Count + 1) + " studenten mee";
            } else {
                widgetData.TodayInfo = "Er wordt niet gekookt vandaag";
            }
            // Get TomorrowInfo
            Meal tomMeal = (Meal) meals.FirstOrDefault(m => m.DateTime.Date == DateTime.Today.AddDays(1).Date);
            if (tomMeal != null) {
                widgetData.TomorrowInfo = "Morgen eten er " + (tomMeal.Guests.Count + 1) + " studenten mee";
            } else {
                widgetData.TomorrowInfo = "Er word nog niet gekookt morgen";
            }
            // Get YouCookToday
            var user = User as ClaimsPrincipal;
            string userId =  user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Student student = studentRepository.GetStudent(userId);
            if (meal != null) {
                if (meal.Cook.Id == student.Id) {
                    widgetData.YouCookToday = "Je moet vandaag koken";
                } else {
                    widgetData.YouCookToday = "Je hoeft vandaag niet te koken";
                }
            } else {
                widgetData.YouCookToday = "Je hoeft vandaag niet te koken";
            }
            // Get YouCook
            int count = meals.Where(m => m.Cook.Id == student.Id).Count();
            widgetData.YouCookTwoWeeks = "Je kookt " + count + " maaltijden";
            count = 0;
            // Get YouEat
            foreach (var ml in meals) {
                Guest studGuest = (Guest) ml.Guests.FirstOrDefault(sg => sg.StudentId == student.Id);
                if (studGuest != null) count++;
            }
            widgetData.YouEatTwoWeeks = "Je eet " + count + " keer mee";

            return View(widgetData);

        }

    }

}
