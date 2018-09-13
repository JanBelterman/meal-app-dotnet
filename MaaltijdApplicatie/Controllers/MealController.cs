using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;
using MaaltijdApplicatie.Models.ViewModels;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
            return View(MealTransformer.TransformMeals(repository.Meals));
        }

        // Renders a view to create a meal
        [Authorize]
        [HttpPost]
        public IActionResult Create(MealDate mealDate) {

            return View(mealDate);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Store(Meal meal) {

            // Validate

            // Store meal or re-render view with errors
            return RedirectToAction("List");

        }

    }

}
