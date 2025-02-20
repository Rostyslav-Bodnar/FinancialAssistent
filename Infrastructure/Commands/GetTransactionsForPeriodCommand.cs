using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetTransactionsForPeriodCommand : IRequest<List<TransactionEntity>> 
    {
        public string Period;

        public GetTransactionsForPeriodCommand(string period)
        {
            Period = period;
        }
    }
}
