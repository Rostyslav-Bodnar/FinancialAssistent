using FinancialAssistent.Entities;

namespace FinancialAssistent.Models
{
    public class DashboardViewModel
    {
        public Entities.User User { get; set; }
        public decimal? Balance { get; set; }
        public bool HasCard { get; set; }

        public List<TransactionEntity> Transactions { get; set; }
    }
}
