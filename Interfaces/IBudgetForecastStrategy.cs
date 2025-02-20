using FinancialAssistent.Services;
using FinancialAssistent.Transfers;

namespace FinancialAssistent.Interfaces
{
    public interface IBudgetForecastStrategy
    {
        Task<BudgetForecastResult> Predict(TransactionService transactionService);
    }
}
