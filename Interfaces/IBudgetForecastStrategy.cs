using FinancialAssistent.Services;
using FinancialAssistent.Transfers;

namespace FinancialAssistent.Interfaces
{
    public interface IBudgetForecastStrategy
    {
        BudgetForecastResult Predict(TransactionService transactionService);
    }
}
