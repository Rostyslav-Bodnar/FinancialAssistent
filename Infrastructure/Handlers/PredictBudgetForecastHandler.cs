using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Interfaces;
using FinancialAssistent.Services;
using FinancialAssistent.Strategy;
using FinancialAssistent.Transfers;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class PredictBudgetForecastHandler : IRequestHandler<PredictBudgetForecastCommand, BudgetForecastResult>
    {

        private readonly TransactionService _transactionService;
        private readonly IDictionary<string, IBudgetForecastStrategy> _strategies;

        public PredictBudgetForecastHandler(TransactionService transactionService)
        {
            _transactionService = transactionService;
            _strategies = new Dictionary<string, IBudgetForecastStrategy>
            {
                { "monthly", new MonthlyForecastStrategy() },
                { "weekly", new WeeklyForecastStrategy() }
            };
        }

        public async Task<BudgetForecastResult> Handle(PredictBudgetForecastCommand request, CancellationToken cancellationToken)
        {
            if (!_strategies.ContainsKey(request.Type))
            {
                throw new ArgumentException("Invalid forecast type.");
            }

            var result = await _strategies[request.Type].Predict(_transactionService);
            return result;
        }
    }
}
