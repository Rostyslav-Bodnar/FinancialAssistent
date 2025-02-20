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
            var bankCards = dbContext.BankCards.Where(c => c.UserId == userId).ToList();
            decimal totalBalance = bankCards.Sum(c => c.Balance);

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
