using FinancialAssistent.Entities;

namespace FinancialAssistent.ViewModels
{
    public class ProfileInfoViewModel
    {
        public User User { get; set; }
        public UserInfo UserInfo { get; set; }
        public List<BankCardEntity> bankCardEntities { get; set; }

        public decimal TotalBalance
        {
            get
            {
                return bankCardEntities?.Sum(card => card.Balance) ?? 0;
            }
        }
    }
}
