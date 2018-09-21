using Microsoft.AspNetCore.Mvc;

namespace MaaltijdApplicatie.Controllers {

    public class ErrorController : Controller {

        public ViewResult Error() => View();

    }

}