using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MaaltijdApplicatie.Controllers {

    public class AccountController : Controller {

        private UserManager<AppUser> userManager;

        public AccountController(UserManager<AppUser> userManager) {
            this.userManager = userManager;
        }

        public ViewResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserViewModel register) {

            if (ModelState.IsValid) {

                AppUser user = new AppUser {
                    UserName = register.Username,
                    Email = register.Email,
                    PhoneNumber = register.Number,
                    Name = register.Name
                };

                IdentityResult result = await userManager.CreateAsync(user, register.Password);

                if (result.Succeeded) {
                    return RedirectToAction("Index");
                } else {
                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View(register);

        }

    }

}
