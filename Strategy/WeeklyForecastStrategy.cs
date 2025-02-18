using FinancialAssistent.Interfaces;
using FinancialAssistent.Services;
using FinancialAssistent.Transfers;

namespace FinancialAssistent.Strategy
{
    public class WeeklyForecastStrategy : IBudgetForecastStrategy
    {
        public BudgetForecastResult Predict(TransactionService transactionService)
        {
            DateTime today = DateTime.UtcNow;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var transactions = transactionService.GetTransactions(startOfWeek, today);
            decimal totalBalance = 6000; //transactionService.GetTotalBalance();

            Dictionary<DayOfWeek, decimal> dailyExpenses = new();

            foreach (var transaction in transactions)
            {
                DateTime transactionDate = DateTimeOffset.FromUnixTimeSeconds(transaction.Time).UtcDateTime;
                DayOfWeek dayOfWeek = transactionDate.DayOfWeek;
                if (!dailyExpenses.ContainsKey(dayOfWeek))
                {
                    dailyExpenses[dayOfWeek] = 0;
                }

                dailyExpenses[dayOfWeek] += transaction.Amount;
            }

            decimal avgDailyExpense = dailyExpenses.Values.Any() ? dailyExpenses.Values.Average() : 0;

            List<string> labels = new();
            List<decimal> values = new();

            for (DateTime day = startOfWeek; day <= endOfWeek; day = day.AddDays(1))
            {
                labels.Add(day.DayOfWeek.ToString().First().ToString());

                totalBalance += day <= today ? dailyExpenses.GetValueOrDefault(day.DayOfWeek, 0) : avgDailyExpense;
                values.Add(totalBalance);
            }

            return new BudgetForecastResult(labels, values);
        }
    }

}
