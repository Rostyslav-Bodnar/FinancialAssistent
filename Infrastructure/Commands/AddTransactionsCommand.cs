using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class AddTransactionsCommand : IRequest
    {
        public BankCardEntity Card;
        public long From;
        public long? To;

        public AddTransactionsCommand(BankCardEntity card, long from, long? to = null)
        {
            Card = card;
            From = from;
            To = to;
        }
    }
}
