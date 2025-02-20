using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using System.Globalization;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetTransactionsForPeriodHandler : IRequestHandler<GetTransactionsForPeriodCommand, List<TransactionEntity>>
    {
        private readonly Database dbContext;

        public GetTransactionsForPeriodHandler(Database dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TransactionEntity>> Handle(GetTransactionsForPeriodCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Period))
            {
                request.Period = DateTime.Now.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
            }

            if (!DateTime.TryParseExact(request.Period, "MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                throw new ArgumentException("Invalid date format. Use 'MMMM yyyy' (e.g., 'May 2025').");
            }

            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            List<TransactionEntity> transactions = dbContext.Transactions
                .AsEnumerable()
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).DateTime >= startDate &&
                            DateTimeOffset.FromUnixTimeSeconds(t.Time).DateTime <= endDate)
                .OrderByDescending(t => t.Time)
                .ToList();
            return transactions;
        }
    }
}
