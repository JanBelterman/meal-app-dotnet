using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;
using MaaltijdApplicatie.Models.ViewModels;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

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
            AppUser user = await GetUser();
            return View(MealTransformer.TransformMeals(repository.GetMeals(), user));
        }

        public async Task<IActionResult> Show(int mealId) {
            AppUser student = await GetUser();
            return View(MealTransformer.TransformIntoMealDate(repository.GetMeal(mealId), student));
        }

        // Renders a view to create a meal
        [Authorize]
        [HttpPost]
        public IActionResult Create(MealDate mealDate) {

            return View(mealDate);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Store(MealDate mealDate) {

            // Get meal
            Meal meal = mealDate.Meal;

            // Validate
            if (ModelState.IsValid) {

                // Get StudentCook
                AppUser student = await GetUser();
                meal.StudentCook = student;

                // Set date with time
                meal.DateTime = new DateTime(meal.DateTime.Year, meal.DateTime.Month, meal.DateTime.Day, mealDate.Time.Hour, mealDate.Time.Minute, 0);

                // Save to repo
                repository.SaveMeal(meal);

            } else {
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
            // Add time to meal
            mealDate.Meal.DateTime = new DateTime(mealDate.Meal.DateTime.Year, mealDate.Meal.DateTime.Month, mealDate.Meal.DateTime.Day, mealDate.Time.Hour, mealDate.Time.Minute, 0);
            repository.SaveMeal(mealDate.Meal);

            // Return view with succes message
            TempData["message"] = "Maaltijd succesvol bijgewerkt";
            return RedirectToAction("List");

        }

        [Authorize]
        public async Task<IActionResult> Register(int mealId) {

            Meal meal = repository.GetMeal(mealId);
            AppUser student = await GetUser();

            repository.RegisterForMeal(meal, student);

            TempData["message"] = "Succesvol aangemeld";
            return RedirectToAction("List");

        }

        [Authorize]
        public async Task<IActionResult> Cancel(int mealId) {

            // Get user
            AppUser student = await GetUser();

            // Sign user out
            if (student != null) {

                // Remove studentMeal record with meal id and user id
                repository.UnsubscribeFromMeal(mealId, GetUserId());

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
