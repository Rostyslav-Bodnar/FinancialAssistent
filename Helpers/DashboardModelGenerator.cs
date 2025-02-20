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
        private readonly BalanceInfoService balanceInfoService;
        private readonly BankCardService bankCardService;

        public DashboardModelGenerator(
            UserInfoService userInfoService,
            MonthlyBudgetService monthlyBudgetService,
            CostLimitsService costLimitsService,
            WidgetService widgetService,
            Database dbContext,
            BalanceInfoService balanceInfoService,
            BankCardService bankCardService)
        {
            this.userInfoService = userInfoService;
            this.monthlyBudgetService = monthlyBudgetService;
            this.costLimitsService = costLimitsService;
            this.widgetService = widgetService;
            this.dbContext = dbContext;
            this.balanceInfoService = balanceInfoService;
            this.bankCardService = bankCardService;
        }

        public async Task<DashboardViewModel?> GenerateDashboardModel()
        {
            var user = userInfoService.GetUser();
            Console.WriteLine("User added");
            if (user == null) return null;

            var bankCard = await dbContext.BankCards
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);
            Console.WriteLine("BankCards added");
            if (bankCard == null) return null;
            return new DashboardViewModel
            {
                User = user,
                BankCards = await bankCardService.GetBankCards(user.Id),
                TotalBalance = balanceInfoService.GetTotalBalance(),
                HasCard = true,
                Transactions = bankCard.Transactions.OrderByDescending(t => t.Time).ToList(),
                monthlyBudgetModel = await monthlyBudgetService.GetMonthlyBudgetInfo(),
                costLimitsModel = await costLimitsService.GetCostsLimits(),
                widgets = await widgetService.GetWidgets(),
                icons = dbContext.Icons.ToList(),
            };
        }
    }
}
