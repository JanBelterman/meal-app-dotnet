using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MaaltijdApplicatie.Controllers {

    [Authorize]
    public class AccountController : Controller {

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AppUserRegisterViewModel register) {

            if (ModelState.IsValid) {

                AppUser user = new AppUser {
                    UserName = register.Username,
                    Email = register.Email,
                    PhoneNumber = register.Number,
                    Name = register.Name
                };

                IdentityResult result = await userManager.CreateAsync(user, register.Password);

                if (result.Succeeded) {
                    TempData["message"] = "Account aangemaakt";
                    return RedirectToAction("List", "Meal");
                } else {
                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View(register);

        }

        [AllowAnonymous]
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppUserLoginViewModel loginModel) {

            // If login form is valid
            if (ModelState.IsValid) {

                // Get user
                AppUser user = await userManager.FindByNameAsync(loginModel.Username);

                // If user is found:
                if (user != null) {

                    // Sign user out from different sessions
                    await signInManager.SignOutAsync();
                    // Attempt to sign user in & when succeeded: redirect user to /Account/Index
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false)).Succeeded) {
                        TempData["message"] = "Ingelogd";
                        return RedirectToAction("List", "Meal");
                    }

                }

            }

            // Pass error message to view
            TempData["login_error"] = "Ongeldige gegevens";
            return View(loginModel);

        }

        // Logs user out
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            TempData["message"] = "Uitgelogd";
            return RedirectToAction("List", "Meal");
        }

    }

}
