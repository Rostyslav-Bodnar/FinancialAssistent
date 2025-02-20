using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Services
{
    public class BankCardService
    {
        private readonly IMediator mediator;

        public BankCardService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<List<BankCardEntity>> GetBankCards(string userId)
        {
            return await mediator.Send(new GetBankCardsCommand(userId));
        }

        public async Task<bool> AddBankCard(string userId, BankCardEntity card)
        {
            return await mediator.Send(new AddBankCardCommand(userId, card));
        }

        public async Task<List<BankCardEntity>> CreateBankCards(string userId, string token, MonobankAccountInfoModel model)
        {
            return await mediator.Send(new CreateBankCardsCommand(userId, token, model));
        }

        public async Task<bool> DeleteBankCard(string userId, int id)
        {
            return await mediator.Send(new DeleteBankCardCommand(userId, id));
        }
    }
}
