namespace FinancialAssistent.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public int Time { get; set; }
        public string Description { get; set; }
        public int Mcc { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; }
    }

}
