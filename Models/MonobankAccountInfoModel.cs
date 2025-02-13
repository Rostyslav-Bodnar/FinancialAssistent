using Newtonsoft.Json;

namespace FinancialAssistent.Models
{
    public class MonobankAccountInfoModel
    {
        [JsonProperty("accounts")]
        public List<MonobankAccount> Accounts { get; set; }
    }

    public class MonobankAccount
    {
        [JsonProperty("balance")]
        public long Balance { get; set; } // В копійках

        [JsonProperty("currencyCode")]
        public int CurrencyCode { get; set; }
    }

}
