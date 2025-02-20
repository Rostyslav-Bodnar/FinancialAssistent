using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetTransactionsCommand : IRequest<List<TransactionEntity>>
    {
        public DateTime start;
        public DateTime end;

        public GetTransactionsCommand(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
