using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class AddBankCardHandler : IRequestHandler<AddBankCardCommand, bool>
    {
        private readonly Database dbContext;

        public AddBankCardHandler(Database dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(AddBankCardCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return false;

            request.Model.UserId = request.UserId;
            dbContext.BankCards.Add(request.Model);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
