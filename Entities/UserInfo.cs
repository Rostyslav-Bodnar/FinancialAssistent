using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAssistent.Entities
{
    public class UserInfo
    {
        public int Id { get; set; }
        public decimal MonthlyBudget { get; set; }
        public List<Widgets> Widgets { get; set; }

        public decimal WeeklyCostsLimits { get; set; }
        public decimal DailyCostsLimits { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
