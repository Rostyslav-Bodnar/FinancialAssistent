using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Services
{
    public class AuthService
    {
        private readonly SignInManager<User> signInManager;
        private readonly IMediator mediator;

        public AuthService(IMediator mediator, SignInManager<User> signInManager)
        {
            this.mediator = mediator;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            return await mediator.Send(new RegisterUserCommand(model));
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
