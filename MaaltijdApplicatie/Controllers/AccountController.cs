using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MaaltijdApplicatie.Controllers {

    [Authorize]
    public class AccountController : Controller {

        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IStudentRepository studentRepository;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IStudentRepository studentRepository) {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.studentRepository = studentRepository;
        }

        // Render register form
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create() {
            return View();
        }

        // Creates and stores a user and student record
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AppUserRegisterViewModel register) {

            // Check if user filled in form correctly
            if (ModelState.IsValid) {

                // Create user
                IdentityUser user = new IdentityUser {
                    UserName = register.Username,
                    Email = register.Email,
                    PhoneNumber = register.Number
                };

                // Store user (hash password in process)
                IdentityResult result = await userManager.CreateAsync(user, register.Password);

                // Check if user was created successfully
                if (result.Succeeded) {

                    // Create student (also link student to user)
                    Student student = new Student() {
                        Name = register.Name,
                        UserId = user.Id
                    };
                    studentRepository.SaveStudent(student);
                    // Redirect and show message
                    TempData["success"] = "Account aangemaakt";
                    return RedirectToAction("List", "Meal");

                } else {
                    // Get errors
                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            // Render register form again and show errors to user
            return View(register);

        }

        // Render login form
        [AllowAnonymous]
        public ViewResult Login() {
            return View();
        }

        // Logs user in
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppUserLoginViewModel loginModel) {

            // Check if user filled in login form correctly
            if (ModelState.IsValid) {

                // Get user
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Username);

                // Check if user is found
                if (user != null) {

                    // Sign user out from different sessions
                    await signInManager.SignOutAsync();

                    // Attempt to sign user in -> when succeeded: redirect user
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false)).Succeeded) {
                        // If user is found and signed in successfully: render main view
                        return RedirectToAction("List", "Meal");
                    }

                }

                // If user is not found or if password was incorrect: show error
                TempData["login_error"] = "Ongeldige gegevens";

            }

            // Render login form and show errors
            return View(loginModel);

        }

        // Logs user out
        public async Task<IActionResult> Logout() {
            // Log user out
            await signInManager.SignOutAsync();
            // Render main view
            return RedirectToAction("List", "Meal");
        }

    }

}
