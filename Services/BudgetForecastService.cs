using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/BudgetForecast")]
    [ApiController]
    public class BudgetForecastService
    {
        private readonly TransactionService _transactionService;

        public BudgetForecastService(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpGet("monthly")]
        public JsonResult PredictMonthlyBalance()
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            
            var transactions = _transactionService.GetTransactions(firstDayOfMonth, today);

            decimal totalBalance = _transactionService.GetTotalBalance();
            totalBalance = 6000; // Тимчасово жорстко заданий баланс

            Dictionary<DateOnly, decimal> weeklyExpenses = new Dictionary<DateOnly, decimal>();

            // Збираємо витрати за минулі тижні
            foreach (var transaction in transactions)
            {
                DateTime transactionDate = DateTimeOffset.FromUnixTimeSeconds(transaction.Time).UtcDateTime;
                DateOnly startOfWeek = GetStartOfWeek(transactionDate, firstDayOfMonth, lastDayOfMonth);

                if (!weeklyExpenses.ContainsKey(startOfWeek))
                {
                    weeklyExpenses[startOfWeek] = 0;
                }

                weeklyExpenses[startOfWeek] += transaction.Amount;
            }

            // Розрахунок середніх витрат на тиждень
            decimal avgWeeklyExpense = weeklyExpenses.Values.Any()
                ? weeklyExpenses.Values.Average()
                : 0;

            List<string> _labels = new List<string>();
            List<decimal> _values = new List<decimal>();

            List<DateOnly> weekStartDates = GetWeekStartDates(firstDayOfMonth, lastDayOfMonth);

            foreach (var weekStart in weekStartDates)
            {
                _labels.Add(weekStart.ToString("dd.MM"));

                if (weekStart <= DateOnly.FromDateTime(today)) // Якщо тиждень уже настав, беремо реальні витрати
                {
                    if (weeklyExpenses.ContainsKey(weekStart))
                    {
                        totalBalance += weeklyExpenses[weekStart]; // Віднімаємо витрати
                    }
                }
                else // Якщо тиждень ще не настав, використовуємо середнє значення витрат
                {
                    totalBalance += avgWeeklyExpense;
                }

                _values.Add(totalBalance);
            }

            var result = new
            {
                labels = _labels,
                values = _values
            };

            return new JsonResult(result);
        }

        private DateOnly GetStartOfWeek(DateTime date, DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            // Отримуємо день тижня для заданої дати
            DayOfWeek dayOfWeek = date.DayOfWeek;

            // Визначаємо початок тижня за стандартними правилами (понеділок – початок, неділя – кінець)
            int diff = (dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1);
            DateOnly weekStart = DateOnly.FromDateTime(date.AddDays(-diff));

            // Якщо тиждень почався в попередньому місяці, то виставляємо перший день місяця
            if (weekStart < DateOnly.FromDateTime(firstDayOfMonth))
            {
                return DateOnly.FromDateTime(firstDayOfMonth);
            }

            return weekStart;
        }

        private List<DateOnly> GetWeekStartDates(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            List<DateOnly> weekStartDates = new List<DateOnly>();

            DateOnly current = DateOnly.FromDateTime(firstDayOfMonth);
            DateOnly lastDay = DateOnly.FromDateTime(lastDayOfMonth);

            // Додаємо перший тиждень
            DayOfWeek firstDayWeek = firstDayOfMonth.DayOfWeek;
            int daysToSunday = (firstDayWeek == DayOfWeek.Sunday) ? 0 : 7 - (int)firstDayWeek;
            DateOnly firstWeekEnd = current.AddDays(daysToSunday);
            weekStartDates.Add(current);

            // Додаємо наступні тижні
            while (firstWeekEnd < lastDay)
            {
                current = firstWeekEnd.AddDays(1);
                weekStartDates.Add(current);

                firstWeekEnd = current.AddDays(6);
                if (firstWeekEnd > lastDay)
                    firstWeekEnd = lastDay;
            }

            return weekStartDates;
        }

        [HttpGet("weekly")]
        public JsonResult PredictWeeklyBalance()
        {
            DateTime today = DateTime.UtcNow;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Початок поточного тижня (неділя)
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var transactions = _transactionService.GetTransactions(startOfWeek, today);

            decimal totalBalance = _transactionService.GetTotalBalance();
            totalBalance = 6000; // Тимчасово жорстко заданий баланс

            Dictionary<DayOfWeek, decimal> dailyExpenses = new Dictionary<DayOfWeek, decimal>();

            foreach (var transaction in transactions)
            {
                DateTime transactionDate = DateTimeOffset.FromUnixTimeSeconds(transaction.Time).UtcDateTime;
                DayOfWeek dayOfWeek = transactionDate.DayOfWeek;

                if (!dailyExpenses.ContainsKey(dayOfWeek))
                {
                    dailyExpenses[dayOfWeek] = 0;
                }

                dailyExpenses[dayOfWeek] += transaction.Amount; // Сума витрат за день
            }

            // Розрахунок середніх витрат на тиждень
            decimal avgDailyExpense = dailyExpenses.Values.Any() ? dailyExpenses.Values.Average() : 0;

            List<char> _labels = new List<char>();
            List<decimal> _values = new List<decimal>();

            // Заповнюємо реальні та прогнозовані витрати
            for (DateTime day = startOfWeek; day <= endOfWeek; day = day.AddDays(1))
            {
                _labels.Add(day.DayOfWeek.ToString().First());

                if (day <= today) // Якщо день уже настав, беремо реальні витрати
                {
                    totalBalance += dailyExpenses.ContainsKey(day.DayOfWeek) ? dailyExpenses[day.DayOfWeek] : 0;
                }
                else // Якщо день ще не настав, використовуємо середнє значення витрат
                {
                    totalBalance += avgDailyExpense;
                }

                _values.Add(totalBalance);
            }

            var result = new
            {
                labels = _labels,
                values = _values
            };

            return new JsonResult(result);
        }
    }
}
