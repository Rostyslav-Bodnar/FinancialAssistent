using Microsoft.AspNetCore.Mvc;
using FinancialAssistent.Services;
using FinancialAssistent.ViewModels;

namespace FinancialAssistent.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {
            var result = await authService.RegisterUserAsync(model.registerModel);

            if (result.Succeeded)
            {
                Console.WriteLine("Result Suceeded");
                return RedirectToAction("AddMonobankCard", "Monobank");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                Console.WriteLine($"Error: {error.Code} - {error.Description}");
            }
            Console.WriteLine("Result is Not Suceeded");
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
            var result = await authService.LoginUserAsync(model.loginModel);

            if (result.Succeeded)
            {
                return RedirectToAction("AddMonobankCard", "Monobank");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            Console.WriteLine("Login error");
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await authService.LogoutUserAsync();
            return RedirectToAction("Auth");
        }
    }
}
