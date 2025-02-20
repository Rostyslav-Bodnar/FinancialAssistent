using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;

namespace FinancialAssistent.Services
{
    public class TransactionService
    {
        private readonly IMediator mediator;

        public TransactionService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<List<TransactionEntity>> GetTransactions(DateTime start, DateTime end)
        {
            return await mediator.Send(new GetTransactionsCommand(start, end));
        }

        public async Task<List<TransactionEntity>> GetTransactionsForPeriod(string period)
        {
            return await mediator.Send(new GetTransactionsForPeriodCommand(period));
        }

        public async Task AddTransactions(BankCardEntity card, long from, long? to = null)
        {
            await mediator.Send(new AddTransactionsCommand(card, from, to));
        }

    }
}
