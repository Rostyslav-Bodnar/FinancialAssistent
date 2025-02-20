using FinancialAssistent.Converters;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using FinancialAssistent.Transfers;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetCategoryExpensesHandler : IRequestHandler<GetCategoryExpencesCommand, CategoryExpencesResult>
    {
        private readonly TransactionService transactionService;

        public GetCategoryExpensesHandler(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<CategoryExpencesResult> Handle(GetCategoryExpencesCommand request, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var transactions = await transactionService.GetTransactions(firstDayOfMonth, today);

            var categoryExpenses = new Dictionary<string, decimal>();

            foreach (var transaction in transactions)
            {
                string category = TransactionCategoryConverter.GetExpenseCategory(transaction.Mcc);

                if (categoryExpenses.ContainsKey(category))
                {
                    categoryExpenses[category] += -transaction.Amount;
                }
                else
                {
                    categoryExpenses[category] = -transaction.Amount;
                }
            }

            List<string> labels = categoryExpenses.Keys.ToList();
            List<decimal> values = categoryExpenses.Values.ToList();

            var result = new CategoryExpencesResult(labels, values);

            return result;
        }
    }
}
