using FinancialAssistent.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    public class TransactionService
    {
        public readonly Database _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionService(Database context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<TransactionEntity> GetTransactions(DateTime start, DateTime end)
        {
            var userId = GetUserId();

            var bankCard = _context.BankCards.FirstOrDefault(c => c.UserId == userId);

            if (bankCard == null)
            {
                return new List<TransactionEntity>();
            }
            return _context.Transactions
                .AsEnumerable()
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime >= start &&
                            DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime <= end &&
                            t.BankCardId == bankCard.Id)
                .ToList();
        }

        public decimal GetTotalBalance()
        {
            var userId = GetUserId();
            var bankCard = _context.BankCards.FirstOrDefault(c => c.UserId == userId);
            decimal totalBalance = (bankCard?.Balance ?? 0) + (bankCard?.User?.UserInfo.Cash ?? 0);

            return totalBalance;
        }

        public decimal GetMonthlyBalance()
        {
            var userId = GetUserId();

            decimal monthlyBudget = _context.Users.Include(ui => ui.UserInfo).FirstOrDefault(u => u.Id == userId).UserInfo.MonthlyBudget;
            return monthlyBudget;
        }

        private string? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return _userManager.GetUserId(user);
        }

        public User? GetUser()
        {
            var userId = GetUserId();
            return userId != null ? _context.Users.Include(u=> u.UserInfo).FirstOrDefault(u => u.Id == userId) : null;
        }
    }
}
