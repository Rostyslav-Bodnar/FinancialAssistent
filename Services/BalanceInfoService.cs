using FinancialAssistent.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    public class BalanceInfoService
    {
        private readonly UserInfoService userInfoService;
        private readonly Database dbContext;
        public BalanceInfoService(UserInfoService userInfoService, Database dbContext)
        {
            this.userInfoService = userInfoService;
            this.dbContext = dbContext;
        }

        public decimal GetTotalBalance()
        {
            var userId = userInfoService.GetUserId();
            var bankCard = dbContext.BankCards.FirstOrDefault(c => c.UserId == userId);
            decimal totalBalance = (bankCard?.Balance ?? 0) + (bankCard?.User?.UserInfo.Cash ?? 0);

            return totalBalance;
        }

        public decimal GetMonthlyBalance()
        {
            var userId = userInfoService.GetUserId();

            decimal monthlyBudget = dbContext.Users.Include(ui => ui.UserInfo).FirstOrDefault(u => u.Id == userId).UserInfo.MonthlyBudget;
            return monthlyBudget;
        }
    }
}
