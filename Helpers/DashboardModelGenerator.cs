using FinancialAssistent.Entities;
using FinancialAssistent.Services;
using FinancialAssistent.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Helpers
{
    public class DashboardModelGenerator
    {
        private readonly UserInfoService userInfoService;
        private readonly MonthlyBudgetService monthlyBudgetService;
        private readonly CostLimitsService costLimitsService;
        private readonly WidgetService widgetService;
        private readonly Database dbContext;

        public DashboardModelGenerator(
            UserInfoService userInfoService,
            MonthlyBudgetService monthlyBudgetService,
            CostLimitsService costLimitsService,
            WidgetService widgetService,
            Database dbContext)
        {
            this.userInfoService = userInfoService;
            this.monthlyBudgetService = monthlyBudgetService;
            this.costLimitsService = costLimitsService;
            this.widgetService = widgetService;
            this.dbContext = dbContext;
        }

        public async Task<DashboardViewModel?> GenerateDashboardModel()
        {
            var user = userInfoService.GetUser();
            if (user == null) return null;

            var bankCard = await dbContext.BankCards
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);

            if (bankCard == null) return null;

            return new DashboardViewModel
            {
                User = user,
                Balance = bankCard.Balance,
                TotalBalance = bankCard.Balance + user.UserInfo.Cash,
                HasCard = true,
                Transactions = bankCard.Transactions.OrderByDescending(t => t.Time).ToList(),
                monthlyBudgetModel = monthlyBudgetService.GetMonthlyBudgetInfo(),
                costLimitsModel = costLimitsService.GetCostsLimits(),
                widgets = widgetService.GetWidgets(),
                icons = dbContext.Icons.ToList(),
            };
        }
    }
}
