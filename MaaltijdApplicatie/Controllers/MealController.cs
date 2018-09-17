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
        public async Task<IActionResult> List() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            return View(MealTransformer.TransformMeals(repository.GetMeals(), user));
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
        [HttpGet]
        public IActionResult Edit(int mealId) {

            Meal meal = repository.GetMeal(mealId);
            string userId = GetUserId();

            // Check if student is cook of that meal
            if (meal.StudentCook.Id != userId) {
                TempData["general_error="] = "Je kan alleen maaltijden wijzigen waar je zelf kok van bent";
                return RedirectToAction("List");
            }

            // Check if no guests have registered
            if (meal.StudentsGuests.Count > 0) {
                TempData["general_error"] = "Je kan alleen maaltijden bijwerken waar nog niemand mee eet";
                return RedirectToAction("List");
            }

            // Transfor single meal to contain all extra mealdate values (or create MealViewModel
            MealDate mealDate = MealTransformer.TransformIntoMealDate(meal);

            return View(mealDate);

        }

        // Updates a meal
        [Authorize]
        [HttpPost]
        public IActionResult Edit(MealDate mealDate) {

            if (!ModelState.IsValid) {
                return View(mealDate);
            }

            Meal meal = repository.GetMeal(mealDate.Meal.Id);
            string userId = GetUserId();

            // Check if student is cook of that meal
            if (meal.StudentCook.Id != userId) {
                TempData["general_error="] = "Je kan alleen maaltijden wijzigen waar je zelf kok van bent";
                return RedirectToAction("List");
            }

            // Check if no guests have registered
            if (meal.StudentsGuests.Count > 0) {
                TempData["general_error"] = "Je kan alleen maaltijden bijwerken waar nog niemand mee eet";
                return RedirectToAction("List");
            }

            // Update
            repository.SaveMeal(mealDate.Meal);

            // Return view with succes message
            TempData["message"] = "Maaltijd succesvol bijgewerkt";
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

        [Authorize]
        [HttpPost]
        public IActionResult Cancel(MealDate mealDate) {

            // Get user
            var user = GetUser();

            // Sign user out
            if (user != null) {

                // Remove studentMeal record with meal id and user id
                repository.UnsubscribeFromMeal(mealDate.Meal.Id, GetUserId());

            }

            // Render list view + succes message
            TempData["message"] = "Succesvol afgemeld";
            return RedirectToAction("List");

        }

        private async Task<AppUser> GetUser() {

            // Get user id
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Return user
            return await userManager.FindByIdAsync(userId);

        }

        private string GetUserId() {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }

}
