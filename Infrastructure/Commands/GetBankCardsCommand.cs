using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetBankCardsCommand : IRequest<List<BankCardEntity>>
    {
        public string UserId;

        public GetBankCardsCommand(string userId)
        {
            UserId = userId;
        }
    }
}
