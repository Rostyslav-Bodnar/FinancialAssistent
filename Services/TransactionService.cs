using FinancialAssistent.Entities;

namespace FinancialAssistent.Services
{
    public class TransactionService
    {
        public readonly Database _context;
        private readonly UserInfoService userInfoService;

        public TransactionService(Database context, UserInfoService userInfoService)
        { 
            _context = context;
            this.userInfoService = userInfoService;
        }

        public List<TransactionEntity> GetTransactions(DateTime start, DateTime end)
        {
            var userId = userInfoService.GetUserId();

            var bankCard = _context.BankCards.FirstOrDefault(c => c.UserId == userId);

            if (bankCard == null)
            {
                return new List<TransactionEntity>();
            }

            List<TransactionEntity> transactions = _context.Transactions
                .AsEnumerable()
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime >= start.ToLocalTime() &&
                            DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime <= end.ToLocalTime() &&
                            t.BankCardId == bankCard.Id)
                .ToList();

            return transactions;
        }
    }
}
