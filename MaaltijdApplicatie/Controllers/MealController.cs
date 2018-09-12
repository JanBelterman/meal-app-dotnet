using Microsoft.AspNetCore.Mvc;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.Logic;

namespace MaaltijdApplicatie.Controllers {

    public class MealController : Controller {

        private IMealRepository repository;

        public MealController(IMealRepository repo) {
            repository = repo;
        }

        public ViewResult List() {

            return View(MealTransformer.TransformMeals(repository.Meals));

        }

    }

}
