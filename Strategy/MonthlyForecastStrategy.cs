using FinancialAssistent.Interfaces;
using FinancialAssistent.Services;
using FinancialAssistent.Transfers;

namespace FinancialAssistent.Strategy
{
    public class MonthlyForecastStrategy : IBudgetForecastStrategy
    {
        public async Task<BudgetForecastResult> Predict(TransactionService transactionService)
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var transactions = await transactionService.GetTransactions(firstDayOfMonth, today);
            decimal totalBalance = 6000; //transactionService.GetTotalBalance();

            Dictionary<DateOnly, decimal> weeklyExpenses = new();
            foreach (var transaction in transactions)
            {
                DateTime transactionDate = DateTimeOffset.FromUnixTimeSeconds(transaction.Time).UtcDateTime;
                DateOnly startOfWeek = DateService.GetStartOfWeek(transactionDate, firstDayOfMonth, lastDayOfMonth);

                if (!weeklyExpenses.ContainsKey(startOfWeek))
                {
                    weeklyExpenses[startOfWeek] = 0;
                }

                weeklyExpenses[startOfWeek] += transaction.Amount;
            }

            decimal avgWeeklyExpense = weeklyExpenses.Values.Any() ? weeklyExpenses.Values.Average() : 0;

            List<string> labels = new();
            List<decimal> values = new();
            List<DateOnly> weekStartDates = DateService.GetWeekStartDates(firstDayOfMonth, lastDayOfMonth);

            foreach (var weekStart in weekStartDates)
            {
                labels.Add(weekStart.ToString("dd.MM"));

                totalBalance += weekStart <= DateOnly.FromDateTime(today)
                    ? weeklyExpenses.GetValueOrDefault(weekStart, 0)
                    : avgWeeklyExpense;

                values.Add(totalBalance);
            }

            return new BudgetForecastResult(labels, values);
        }
    }

}
