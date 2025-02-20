using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class DeleteBankCardHandler : IRequestHandler<DeleteBankCardCommand, bool>
    {
        private readonly Database dbContext;

        public DeleteBankCardHandler(Database dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteBankCardCommand request, CancellationToken cancellationToken)
        {
            var card = await dbContext.BankCards.FirstOrDefaultAsync(c => c.Id == request.BankId && c.UserId == request.UserId);
            if (card == null)
                return false;

            dbContext.BankCards.Remove(card);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
