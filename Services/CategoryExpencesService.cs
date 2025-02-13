using FinancialAssistent.Entities;
using FinancialAssistent.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/CategoryExpences")]
    [ApiController]
    public class CategoryExpencesService
    {
        private readonly Database _context;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryExpencesService(Database context, UserManager<User> userManager, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("category")]
        public JsonResult GetCategoryExpences()
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var transactions = GetTransactions(today, firstDayOfMonth);

            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            foreach (var transaction in transactions)
            {
                labels.Add(TransactionCategoryValidator.GetCategory(transaction.Mcc));
                values.Add(-transaction.Amount);
            }

            var result = new
            {
                labels = labels,
                values = values,
            };

            return new JsonResult(result);
        }

        private List<TransactionEntity> GetTransactions(DateTime today, DateTime start)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = userManager.GetUserId(user);
            var bankCard = _context.BankCards.FirstOrDefault(c => c.UserId == userId);

            var transactions = _context.Transactions
                .AsEnumerable()
                .Where(t => DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime >= start &&
                            DateTimeOffset.FromUnixTimeSeconds(t.Time).UtcDateTime <= today
                            && t.BankCardId == bankCard.Id)
                .ToList();
            return transactions;
        }
    }
}
