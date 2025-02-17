using FinancialAssistent.Models;
using FinancialAssistent.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinancialAssistent.Services;

namespace FinancialAssistent.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly WidgetService widgetService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, 
            WidgetService widgetService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.widgetService = widgetService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {
            var user = new User { UserName = model.registerModel.Name, Email = model.registerModel.Email};
            var result = await _userManager.CreateAsync(user, model.registerModel.Password);
            if (result.Succeeded)
            {
                var userInfo = new UserInfo { UserId = user.Id };

                // Додаємо UserInfo в базу після успішного створення User
                var dbContext = HttpContext.RequestServices.GetService<Database>();
                dbContext.UsersInfo.Add(userInfo);
                await dbContext.SaveChangesAsync();
                await widgetService.AddStandartWidgets();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Dashboard", "Monobank");
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
