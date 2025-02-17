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

        public MonthlyBudgetModel monthlyBudgetModel { get; set; }
        public CostsLimitsModel costLimitsModel { get; set; }

        public List<Widgets> widgets { get; set; }
        public List<Icons> icons { get; set; }
    }
}
