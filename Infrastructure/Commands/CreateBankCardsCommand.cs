using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class CreateBankCardsCommand : IRequest<List<BankCardEntity>>
    {
        public string UserId { get; }
        public string Token { get; }
        public MonobankAccountInfoModel Model { get; }

        public CreateBankCardsCommand(string userId, string token, MonobankAccountInfoModel model)
        {
            UserId = userId;
            Token = token;
            Model = model;
        }
    }
}
