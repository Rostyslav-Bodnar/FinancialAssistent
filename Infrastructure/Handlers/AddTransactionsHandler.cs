using FinancialAssistent.Entities;
using FinancialAssistent.Helpers;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class AddTransactionsHandler : IRequestHandler<AddTransactionsCommand>
    {
        private readonly Database dbContext;
        private readonly MonobankHttpClient monobankHttpClient;

        public AddTransactionsHandler(Database dbContext, MonobankHttpClient monobankHttpClient)
        {
            this.dbContext = dbContext;
            this.monobankHttpClient = monobankHttpClient;
        }

        public async Task Handle(AddTransactionsCommand request, CancellationToken cancellationToken)
        {
            request.To ??= DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var transactions = await monobankHttpClient.SendRequest<List<TransactionEntity>>(
                $"https://api.monobank.ua/personal/statement/0/{request.From}/{request.To}",
            request.Card.Token);

            if (transactions == null || transactions.Count == 0) return;

            var existingIds = dbContext.Transactions.Select(t => t.Id).ToHashSet();
            var newTransactions = transactions
                .Where(t => !existingIds.Contains(t.Id))
            .ToList();

            if (newTransactions.Any())
            {
                dbContext.Transactions.AddRange(newTransactions);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
