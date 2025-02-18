using FinancialAssistent.Interfaces;
using FinancialAssistent.Strategy;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/BudgetForecast")]
    [ApiController]
    public class BudgetForecastService
    {
        private readonly TransactionService transactionService;

        private readonly IDictionary<string, IBudgetForecastStrategy> _strategies;

        public BudgetForecastService(TransactionService transactionProvider)
        {
            transactionService = transactionProvider;
            _strategies = new Dictionary<string, IBudgetForecastStrategy>
        {
            { "monthly", new MonthlyForecastStrategy() },
            { "weekly", new WeeklyForecastStrategy() }
        };
        }

        [HttpGet("{type}")]
        public IResult Predict(string type)
        {
            if (!_strategies.ContainsKey(type))
            {
                return Results.BadRequest("Invalid forecast type.");
            }

            var result = _strategies[type].Predict(transactionService);
            return Results.Ok(result);
        }
    }
}
