using FinancialAssistent.Models;
using FinancialAssistent.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {

            var user = new User { UserName = model.registerModel.Name, Email = model.registerModel.Email };
            var result = await _userManager.CreateAsync(user, model.registerModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Home", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                Console.WriteLine($"Error: {error.Code} - {error.Description}"); // Виводить всі помилки в консоль
            }

            return View("Auth", model);
        }


        [HttpGet]
        public IActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.loginModel.Username, model.loginModel.Password, model.loginModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard", "Monobank");

            }
            ModelState.AddModelError("", "Invalid login attempt.");
            Console.WriteLine($"Error"); // Виводить всі помилки в консоль
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Auth");
        }
    }
}
