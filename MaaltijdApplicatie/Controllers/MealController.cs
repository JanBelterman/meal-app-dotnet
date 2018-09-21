using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;
using MaaltijdApplicatie.Models.ViewModels;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;

namespace MaaltijdApplicatie.Controllers {

    [Authorize]
    public class MealController : Controller {
        
        private IMealRepository mealRepository;
        private IStudentRepository studentRepository;

        public MealController(IMealRepository mealRepository, IStudentRepository studentRepository) {
            this.mealRepository = mealRepository;
            this.studentRepository = studentRepository;
        }

        // Renders a list with meals for coming 2 weeks
        [AllowAnonymous]
        public ViewResult List() {
            // Get student from logged in user
            Student student = studentRepository.GetStudent(GetUserId());
            ViewBag.UserIsLoggedIn = student != null;
            // Render view with list of meals transformed into meal dates
            return View(MealTransformer.TransformMeals(mealRepository.GetMeals(), student));
        }

        // Render a single meal
        [AllowAnonymous]
        public IActionResult Show(int mealId) {
            // Get student from logged in user
            Student student = studentRepository.GetStudent(GetUserId());
            ViewBag.UserIsLoggedIn = student != null;
            // Render view with meal transformed into meal date
            return View(MealTransformer.TransformIntoMealDate(mealRepository.GetMeal(mealId), student));
        }

        // Renders a view to create a meal
        [HttpPost]
        public IActionResult Create(MealDate mealDate) =>
            View(mealDate);
        
        // Stores a meal
        [HttpPost]
        public IActionResult Store(MealDate mealDate) {

            // Validate form
            if (ModelState.IsValid) {

                // Get meal
                Meal meal = mealDate.Meal;
                // Combine date and time
                meal.DateTime = new DateTime(meal.DateTime.Year, meal.DateTime.Month, meal.DateTime.Day, mealDate.Time.Hour, mealDate.Time.Minute, 0);
                // Set student cook
                Student student = studentRepository.GetStudent(GetUserId());
                meal.Cook = student;

                // Save to repo
                mealRepository.SaveMeal(meal);

            } else {
                // If form is not valid return from to user and pass meal date to fill in values
                return View("Create", mealDate);
            }

            // Return user to main view and show success message
            TempData["message"] = "Maaltijd aangemaakt";
            return RedirectToAction("List");

        }
        
        // Renders a view to edit a meal
        [HttpGet]
        public IActionResult Edit(int mealId) {

            // Get meal
            Meal meal = mealRepository.GetMeal(mealId);
            // Get student
            Student student = studentRepository.GetStudent(GetUserId());

            // Check if meal can be edited by student
            DomainMethodResult result = meal.AllowedToEdit(student);

            // Render edit view
            if (result.WasSuccessful) {
                // Render edit view with => pass meal date to fill in values
                return View(MealTransformer.TransformIntoMealDate(meal));
            }
            // Render main view -> Show error message
            else {
                TempData["general_error"] = result.Message;
                return RedirectToAction("List");
            }

        }

        // Updates a meal
        [HttpPost]
        public IActionResult Edit(MealDate mealDate) {

            // Check if user filled in form correctly
            if (!ModelState.IsValid) {
                // If not => return edit view again => send meal date to fill in values
                return View(mealDate);
            }

            // Get meal
            Meal meal = mealRepository.GetMeal(mealDate.Meal.Id);
            // Get student
            Student student = studentRepository.GetStudent(GetUserId());

            // Check if meal can be edited by student
            DomainMethodResult result = meal.AllowedToEdit(student);

            // Meal cannot be edited by student -> add with error
            if (!result.WasSuccessful) {
                TempData["general_error"] = result.Message;
            }
            // Meal can be edited by student -> update meal -> add success message
            else {
                // Combine date and time
                mealDate.Meal.DateTime = new DateTime(mealDate.Meal.DateTime.Year, mealDate.Meal.DateTime.Month, mealDate.Meal.DateTime.Day, mealDate.Time.Hour, mealDate.Time.Minute, 0);
                // Update meal in database
                mealRepository.SaveMeal(mealDate.Meal);
                TempData["message"] = "Maaltijd succesvol bijgewerkt";
            }

            return RedirectToAction("List");

        }
        
        // Adds a student to guests of a meal
        public IActionResult Join(int mealId) {

            // Get meal
            Meal meal = mealRepository.GetMeal(mealId);
            // Get student
            Student student = studentRepository.GetStudent(GetUserId());

            // Attemt to join student in meal
            DomainMethodResult result = meal.Join(student);

            // Save to database -> show success message
            if (result.WasSuccessful) {
                mealRepository.Save();
                TempData["message"] = result.Message;
            }
            // Show error message
            else {
                TempData["general_error"] = result.Message;
            }

            // Render main view
            return RedirectToAction("List");

        }
        
        // Removes student from guests of a meal
        public IActionResult Leave(int mealId) {

            // Get student
            Student student = studentRepository.GetStudent(GetUserId());
            // Get meal
            Meal meal = mealRepository.GetMeal(mealId);

            // Attempt to leave student from meal
            DomainMethodResult result = meal.Leave(student);
            // Save to database -> show success message
            if (result.WasSuccessful) {
                mealRepository.Save();
                TempData["message"] = result.Message;
            }
            // Show error message
            else {
                TempData["general_error"] = result.Message;
            }

            // Render main view
            return RedirectToAction("List");

        }

        // Get user id from cookies
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

    }

}
