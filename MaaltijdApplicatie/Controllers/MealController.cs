using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;
using MaaltijdApplicatie.Models.ViewModels;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MaaltijdApplicatie.Controllers {

    public class MealController : Controller {

        private IMealRepository repository;
        private UserManager<AppUser> userManager;

        public MealController(IMealRepository repo, UserManager<AppUser> userMng) {
            repository = repo;
            userManager = userMng;
        }

        // Renders a list with meals for coming 2 weeks
        public ViewResult List() {
            return View(MealTransformer.TransformMeals(repository.GetMeals()));
        }

        // Renders a view to create a meal
        [Authorize]
        [HttpPost]
        public IActionResult Create(MealDate mealDate) {

            return View(mealDate);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Store(Meal meal) {

            // Validate
            if (ModelState.IsValid) {

                // Get user (student) who is cook
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(userId);
                meal.StudentCook = user;

                // Save to repo
                repository.SaveMeal(meal);

            } else {
                var mealDate = new MealDate() {
                    Meal = meal,
                    Date = meal.DateTime
                };
                MealTransformer.AddDateStrings(mealDate); // Does it add reference?
                return View("Create", mealDate);
            }

            // Store meal or re-render view with errors
            TempData["message"] = "Maaltijd aangemaakt";
            return RedirectToAction("List");

        }

        [Authorize]
        public async Task<IActionResult> Register(MealDate mealDate) {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            repository.RegisterForMeal(mealDate.Meal, user);

            TempData["message"] = "Succesvol aangemeld";
            return RedirectToAction("List");

        }

    }

}
