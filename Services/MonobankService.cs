using FinancialAssistent.Helpers;
using FinancialAssistent.Models;

namespace FinancialAssistent.Services
{
    public class MonobankService
    {
        private readonly MonobankHttpClient monobankHttpClient;

        public MonobankService(MonobankHttpClient monobankHttpClient)
        {
            this.monobankHttpClient = monobankHttpClient;
        }

        public async Task<MonobankAccountInfoModel> GetAccountInfo(string token)
        {
            return await monobankHttpClient.SendRequest<MonobankAccountInfoModel>("https://api.monobank.ua/personal/client-info", token);
        }

    }
}