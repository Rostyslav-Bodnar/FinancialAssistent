using FinancialAssistent.Entities;

namespace FinancialAssistent.Models
{
    public class DashboardViewModel
    {
        public User User { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal? Balance { get; set; }
        public bool HasCard { get; set; }
        public List<TransactionEntity> Transactions { get; set; }

        public decimal MonthBudget { get; set; }
    }
}
