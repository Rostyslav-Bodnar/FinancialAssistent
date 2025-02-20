using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly Database dbContext;

        public RegisterUserHandler(UserManager<User> userManager, SignInManager<User> signInManager, Database dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var model = request.model;
            var user = new User { UserName = model.Name, Email = model.Email };

            var result = await userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                var userInfo = new UserInfo { UserId = user.Id };
                dbContext.UsersInfo.Add(userInfo);
                await dbContext.SaveChangesAsync();
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }
    }
}
