using System.Linq;
using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    public interface IMealRepository {

        IQueryable<Meal> Meals { get; }

    }

}
