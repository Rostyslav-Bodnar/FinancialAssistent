using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetBankCardsHandler : IRequestHandler<GetBankCardsCommand, List<BankCardEntity>>
    {
        private readonly Database dbContext;

        public GetBankCardsHandler(Database dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<BankCardEntity>> Handle(GetBankCardsCommand request, CancellationToken cancellationToken)
        {
            return await dbContext.BankCards.Where(c => c.UserId == request.UserId).ToListAsync();
        }
    }
}
