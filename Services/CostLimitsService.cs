using FinancialAssistent.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/CategoryExpences")]
    [ApiController]
    public class CostLimitsService
    {
        private readonly TransactionService transactionService;

        public CostLimitsService(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet("getLimits")]
        public CostsLimitsModel GetCostsLimits()
        {
            DateTime today = DateTime.UtcNow;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1);
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var transactions = transactionService.GetTransactions(firstDayOfMonth, today);

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

            return new CostsLimitsModel
            {
                DailyLimit = adjustedDailyLimit,
                WeeklyLimit = weeklyLimit,
                DailySpent = dailySpent,
                WeeklySpent = weeklySpent
            };
        }

        [HttpPost("setLimits")]
        public async Task<IResult> SetCostsLimits([FromBody] CostsLimitsModel model)
        {
            if (model == null)
            {
                return Results.BadRequest("Invalid budget data.");
            }

            var user = transactionService.GetUser();
            if (user == null)
            {
                return Results.Unauthorized();
            }


            return Results.Ok(new { message = "Monthly budget updated successfully", budget = user.UserInfo.MonthlyBudget });
        }

    }
}
