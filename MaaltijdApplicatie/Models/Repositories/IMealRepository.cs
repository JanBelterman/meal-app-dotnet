using System.Linq;
using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    interface IMealRepository {

        IQueryable<Meal> Meals { get; }

    }

}
