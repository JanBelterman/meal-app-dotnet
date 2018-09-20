using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MaaltijdApplicatie.Components {

    public class WidgetViewComponent : ViewComponent {

        public IMealRepository repository;
        public UserManager<AppUser> userManager;

        public WidgetViewComponent(IMealRepository repo, UserManager<AppUser> usermng) {
            repository = repo;
            userManager = usermng;
        }

        public IViewComponentResult Invoke() {

            WidgetViewModel widgetData = new WidgetViewModel();

            IQueryable<Meal> meals = repository.GetMeals(DateTime.Today, DateTime.Today.AddDays(14));

            // Get TodayInfo
            Meal meal = (Meal) meals.FirstOrDefault(m => m.DateTime.Date == DateTime.Today.Date);
            if (meal != null) {
                widgetData.TodayInfo = "Vandaag eten er " + (meal.StudentsGuests.Count + 1) + " studenten mee";
            } else {
                widgetData.TodayInfo = "Er wordt niet gekookt vandaag";
            }
            // Get TomorrowInfo
            Meal tomMeal = (Meal) meals.FirstOrDefault(m => m.DateTime.Date == DateTime.Today.AddDays(1).Date);
            if (tomMeal != null) {
                widgetData.TomorrowInfo = "Morgen eten er " + (tomMeal.StudentsGuests.Count + 1) + " studenten mee";
            } else {
                widgetData.TomorrowInfo = "Er word nog niet gekookt morgen";
            }
            // Get YouCookToday
            string userId = GetUserId();
            if (meal != null) {
                if (meal.StudentCook.Id == userId) {
                    widgetData.YouCookToDay = "Je kookt vandaag";
                } else {
                    widgetData.YouCookToDay = "Je kookt vandaag niet";
                }
            }
            // Get YouCook
            int count = meals.Where(m => m.StudentCook.Id == userId).Count();
            widgetData.YouCookTwoWeeks = "Je kookt " + count + " maaltijden";
            count = 0;
            // Get YouEat
            foreach (var ml in meals) {
                StudentGuest studGuest = (StudentGuest) ml.StudentsGuests.FirstOrDefault(sg => sg.AppUserId == userId);
                if (studGuest != null) count++;
            }
            widgetData.YouEatTwoWeeks = "Je eet " + count + " keer mee";

            return View(widgetData);

        }

        private string GetUserId() {

            var user = User as ClaimsPrincipal;

            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }

    }

}
