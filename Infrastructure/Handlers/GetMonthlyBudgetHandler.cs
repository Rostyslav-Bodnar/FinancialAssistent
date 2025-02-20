using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetMonthlyBudgetHandler : IRequestHandler<GetMonthlyBudgetCommand, MonthlyBudgetModel>
    {
        private readonly TransactionService transactionService;
        private readonly BalanceInfoService balanceInfoService;

        public GetMonthlyBudgetHandler(TransactionService transactionService, BalanceInfoService balanceInfoService)
        {
            this.transactionService = transactionService;
            this.balanceInfoService = balanceInfoService;
        }

        public async Task<MonthlyBudgetModel> Handle(GetMonthlyBudgetCommand request, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var transactions = await transactionService.GetTransactions(firstDayOfMonth, today);

            decimal monthlyBudget = balanceInfoService.GetMonthlyBalance();
            decimal spend = 0;
            decimal left = 0;

            foreach (var transaction in transactions)
            {
                spend -= transaction.Amount;
            }
            monthlyBudget = 10000; // грубо задане значення
            left = monthlyBudget - spend;

            var result = new MonthlyBudgetModel
            {
                MonthlyBudget = monthlyBudget,
                SpendedBudget = spend,
                RemainingBudget = left
            };

            return result;
        }
    }
}
