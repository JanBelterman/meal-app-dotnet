using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;
using MaaltijdApplicatie.Models.ViewModels;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;

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

            // Check if student is cook of that meal, I.E. if user is allowed to edit meal
            if (meal.Cook.Id != student.Id) {
                TempData["general_error="] = "Je kan alleen maaltijden wijzigen waar je zelf kok van bent";
                return RedirectToAction("List");
            }

            // Check if no guests have registered, I.E. if user is allowed to edit meal
            if (meal.Guests.Count > 0) {
                TempData["general_error"] = "Je kan alleen maaltijden bijwerken waar nog niemand mee eet";
                return RedirectToAction("List");
            }

            // Render edit view with => pass meal date to fill in values
            return View(MealTransformer.TransformIntoMealDate(meal));

        }

        // Updates a meal
        [HttpPost]
        public IActionResult Edit(MealDate mealDate) {

            // Check if user filled in form correctly
            if (!ModelState.IsValid) {
                // If not => return edit view again => send meal date to fill in values
                return View(mealDate);
            }

            // Get meal => to validate cook and guest properties
            Meal meal = mealRepository.GetMeal(mealDate.Meal.Id);
            // Get student
            Student student = studentRepository.GetStudent(GetUserId());

            // Check if student is cook of that meal, I.E. if user is allowed to edit meal
            if (meal.Cook.Id != student.Id) {
                TempData["general_error="] = "Je kan alleen maaltijden wijzigen waar je zelf kok van bent";
                return RedirectToAction("List");
            }

            // Check if no guests have registered, I.E. if user is allowed to edit meal
            if (meal.Guests.Count > 0) {
                TempData["general_error"] = "Je kan alleen maaltijden bijwerken waar nog niemand mee eet";
                return RedirectToAction("List");
            }
            
            // Combine date and time
            mealDate.Meal.DateTime = new DateTime(mealDate.Meal.DateTime.Year, mealDate.Meal.DateTime.Month, mealDate.Meal.DateTime.Day, mealDate.Time.Hour, mealDate.Time.Minute, 0);
            // Update meal in database
            mealRepository.SaveMeal(mealDate.Meal);

            // Render main view -> show success message
            TempData["message"] = "Maaltijd succesvol bijgewerkt";
            return RedirectToAction("List");

        }
        
        // Adds a student to guests of a meal
        public IActionResult Join(int mealId) {

            // Get meal
            Meal meal = mealRepository.GetMeal(mealId);
            // Get student
            Student student = studentRepository.GetStudent(GetUserId());

            // Check if student is not the cook of the meal
            if (meal.Cook.Id == student.Id) {
                TempData["general_error"] = "Je kan niet mee eten aan een maaltijd die je zelf kookt";
                return RedirectToAction("List");
            }

            // Check if meal has free space
            if (meal.Guests.Count >= meal.MaxGuests) {
                TempData["general_error"] = "Je kan niet meer mee eten, alle plekken zijn bezet";
                return RedirectToAction("List");
            }

            // Add user to guests
            mealRepository.JoinMeal(meal, student);

            // Render main view -> show success message
            TempData["message"] = "Succesvol aangemeld";
            return RedirectToAction("List");

        }
        
        // Removes student from guests of a meal
        public IActionResult Leave(int mealId) {

            // Get student
            Student student = studentRepository.GetStudent(GetUserId());
            // Get meal
            Meal meal = mealRepository.GetMeal(mealId);

            // Check if user is a student & if student has joined this meal
            if (student == null) {
                TempData["general_error"] = "Je kan je niet afmelden voor een maaltijd waar je niet voor aangemeld bent";
            } else if (!meal.Guests.Any(m => m.StudentId == student.Id)) {
                TempData["general_error"] = "Je kan je niet afmelden voor een maaltijd waar je niet voor aangemeld bent";
            } else {
                // User has joined -> student can leave meal -> add succes message
                mealRepository.LeaveMeal(mealId, student.Id);
                TempData["message"] = "Succesvol afgemeld";
            }

            // Render main view -> show error of succes message
            return RedirectToAction("List");

        }

        // Get user id from cookies
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

    }

}
