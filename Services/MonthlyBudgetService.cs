using FinancialAssistent.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/MonthlyBudget")]
    [ApiController]
    public class MonthlyBudgetService
    {
        private readonly TransactionService transactionService;

        public MonthlyBudgetService(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }


        [HttpGet("budget")]
        public MonthlyBudgetModel GetMonthlyBudgetInfo()
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var transactions = transactionService.GetTransactions(firstDayOfMonth, today);

            decimal monthlyBudget = transactionService.GetMonthlyBalance();
            decimal spend = 0;
            decimal left = 0;

            foreach(var transaction in transactions)
            {
                spend -= transaction.Amount;
            }
            monthlyBudget = 10000;
            left = monthlyBudget - spend;
            Console.WriteLine(monthlyBudget);
            Console.WriteLine(left);
            Console.WriteLine(spend);

            return new MonthlyBudgetModel
            {
                MonthlyBudget = monthlyBudget,
                SpendedBudget = spend,
                RemainingBudget = left
            };
        }

        [HttpPost("setBudget")]
        public async Task<IResult> SetMonthlyBudget([FromBody] MonthlyBudgetModel model)
        {
            if (model == null)
            {
                return Results.BadRequest("Invalid budget data.");
            }

            decimal maxBudget = transactionService.GetTotalBalance();
            if (model.MonthlyBudget > maxBudget)
            {
                return Results.BadRequest($"Budget cannot exceed {maxBudget}.");
            }

            var user = transactionService.GetUser();
            if (user == null)
            {
                return Results.Unauthorized();
            }

            user.UserInfo.MonthlyBudget = model.MonthlyBudget;
            await transactionService._context.SaveChangesAsync();

            return Results.Ok(new { message = "Monthly budget updated successfully", budget = user.UserInfo.MonthlyBudget });
        }


    }

}
