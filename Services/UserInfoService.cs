using FinancialAssistent.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    public class UserInfoService
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Database dbContext;

        public UserInfoService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, Database dbContext)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public string? GetUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            return userManager.GetUserId(user);
        }

        public User? GetUser()
        {
            var userId = GetUserId();
            return userId != null ? dbContext.Users.Include(u => u.UserInfo).FirstOrDefault(u => u.Id == userId) : null;
        }
    }
}
