using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsCommand, List<TransactionEntity>>
    {
        private readonly UserInfoService userInfoService;
        private readonly Database dbContext;

        public GetTransactionsHandler(UserInfoService userInfoService, Database dbContext)
        {
            this.userInfoService = userInfoService;
            this.dbContext = dbContext;
        }

        public async Task<List<TransactionEntity>> Handle(GetTransactionsCommand request, CancellationToken cancellationToken)
        {
            var userId = userInfoService.GetUserId();

            var bankCard = dbContext.BankCards.FirstOrDefault(c => c.UserId == userId);

            if (bankCard == null)
            {
                return new List<TransactionEntity>();
            }

            List<TransactionEntity> transactions = dbContext.Transactions
                .AsEnumerable()
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime >= request.start.ToLocalTime() &&
                            DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime <= request.end.ToLocalTime() &&
                            t.BankCardId == bankCard.Id)
                .ToList();

            return transactions;
        }
    }
}
