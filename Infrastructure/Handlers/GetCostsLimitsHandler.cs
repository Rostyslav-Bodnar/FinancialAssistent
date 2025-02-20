using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetCostsLimitsHandler : IRequestHandler<GetCostsLimitsCommand, CostsLimitsModel>
    {
        private readonly TransactionService transactionService;

        public GetCostsLimitsHandler(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<CostsLimitsModel> Handle(GetCostsLimitsCommand request, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.UtcNow.ToLocalTime();
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1);
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var transactions = await transactionService.GetTransactions(firstDayOfMonth, today);

            decimal dailySpent = transactions
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime.Date == today.Date)
                .Sum(t => -t.Amount);

            decimal weeklySpent = transactions
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime >= startOfWeek)
                .Sum(t => -t.Amount);

            decimal DailyLimitBase = 200; // тимчасове значення
            decimal weeklyLimit = DailyLimitBase * 7;
            int daysPassed = (today - startOfWeek).Days + 1;

            decimal avgDailySpent = daysPassed > 0 ? weeklySpent / daysPassed : 0;
            decimal projectedWeeklySpent = avgDailySpent * 7;
            decimal adjustedDailyLimit = DailyLimitBase;

            if (projectedWeeklySpent > weeklyLimit)
            {
                adjustedDailyLimit = Math.Max(0, (weeklyLimit - weeklySpent) / (7 - daysPassed));
            }

            var result = new CostsLimitsModel
            {
                DailyLimit = adjustedDailyLimit,
                WeeklyLimit = weeklyLimit,
                DailySpent = dailySpent,
                WeeklySpent = weeklySpent
            };
            return result;

        }
    }
}
