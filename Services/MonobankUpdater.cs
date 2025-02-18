using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace FinancialAssistent.Services
{
    public class MonobankUpdater
    {
        private readonly MonobankService monobankService;
        private readonly Database dbContext;

        public MonobankUpdater(MonobankService monobankService, Database dbContext)
        {
            this.monobankService = monobankService;
            this.dbContext = dbContext;
        }

        public async Task UpdateUserData(string userId, string token)
        {
            var account = await monobankService.GetAccountInfo(token);
            var bankCard = await monobankService.GetOrCreateBankCard(userId, token);
            bankCard.Balance = account.Accounts[0].Balance / 100m;

            await monobankService.AddTransactions(bankCard);
            await dbContext.SaveChangesAsync();
        }
    }
}
