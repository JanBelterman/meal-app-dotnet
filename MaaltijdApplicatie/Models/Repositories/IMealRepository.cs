using System.Linq;
using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    public interface IMealRepository {

        IQueryable<Meal> GetMeals();

        void SaveMeal(Meal meal);

        void RegisterForMeal(Meal meal, AppUser student);

    }

}
