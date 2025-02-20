using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class AddBankCardCommand : IRequest<bool>
    {
        public string UserId;
        public BankCardEntity Model;

        public AddBankCardCommand(string userId, BankCardEntity model)
        {
            UserId = userId;
            Model = model;
        }
    }
}
