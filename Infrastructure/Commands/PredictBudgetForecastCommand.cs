using FinancialAssistent.Transfers;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class PredictBudgetForecastCommand : IRequest<BudgetForecastResult>
    {
        public string Type { get; }

        public PredictBudgetForecastCommand(string type)
        {
            Type = type;
        }
    }
}
