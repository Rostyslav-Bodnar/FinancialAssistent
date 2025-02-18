using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Services
{
    public class AuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly Database dbContext;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, Database dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new User { UserName = model.Name, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var userInfo = new UserInfo { UserId = user.Id };
                dbContext.UsersInfo.Add(userInfo);
                await dbContext.SaveChangesAsync();
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(LoginModel model)
        {
            return await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task LogoutUserAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
