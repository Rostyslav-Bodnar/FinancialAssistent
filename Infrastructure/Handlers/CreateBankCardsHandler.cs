using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;
namespace FinancialAssistent.Infrastructure.Handlers
{
    public class CreateBankCardsHandler :
        IRequestHandler<CreateBankCardsCommand, List<BankCardEntity>>
    {
        public Database dbContext;

        public CreateBankCardsHandler(Database dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<BankCardEntity>> Handle(CreateBankCardsCommand request, CancellationToken cancellationToken)
        {
            List<BankCardEntity> bankCards = new List<BankCardEntity>();
            foreach (var ac in request.Model.Accounts)
            {
                var newBankCard = new BankCardEntity { UserId = request.UserId, Token = request.Token, MaskedPan = ac.MaskedPan[0] };
                dbContext.BankCards.Add(newBankCard);
                await dbContext.SaveChangesAsync();

                bankCards.Add(newBankCard);
            }
            return bankCards;
        }
    }
}
