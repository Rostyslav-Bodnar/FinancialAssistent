using FinancialAssistent.Converters;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/CategoryExpences")]
    [ApiController]
    public class CategoryExpencesService
    {
        private readonly TransactionService transactionService;

        public CategoryExpencesService(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }


        [HttpGet("category")]
        public JsonResult GetCategoryExpences()
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var transactions = transactionService.GetTransactions(firstDayOfMonth, today);

            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            foreach (var transaction in transactions)
            {
                labels.Add(TransactionCategoryConverter.GetCategory(transaction.Mcc));
                values.Add(-transaction.Amount);
            }

            var result = new
            {
                labels = labels,
                values = values,
            };

            return new JsonResult(result);
        }
    }
}
