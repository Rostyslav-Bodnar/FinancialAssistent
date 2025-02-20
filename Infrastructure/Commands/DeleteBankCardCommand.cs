using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class DeleteBankCardCommand : IRequest<bool>
    {
        public string UserId;
        public int BankId;

        public DeleteBankCardCommand(string userId, int bankId)
        {
            UserId = userId;
            BankId = bankId;
        }
    }
}
