using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinancialAssistent.Services
{
    public class MonobankService
    {
        private readonly HttpClient _httpClient;
        private readonly Database _dbContext;

        public MonobankService(HttpClient httpClient, Database dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }

        public async Task<MonobankAccountInfoModel> GetAccountInfo(string token)
        {
            return await SendRequest<MonobankAccountInfoModel>("https://api.monobank.ua/personal/client-info", token);
        }

        public async Task<BankCardEntity> GetOrCreateBankCard(string userId, string token)
        {
            var bankCard = await _dbContext.BankCards
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (bankCard == null)
            {
                bankCard = new BankCardEntity { UserId = userId, Token = token };
                _dbContext.BankCards.Add(bankCard);
                await _dbContext.SaveChangesAsync();
            }
            return bankCard;
        }

        public async Task AddTransactions(BankCardEntity card)
        {
            long fromTime = DateTimeOffset.UtcNow.AddMonths(-1).ToUnixTimeSeconds();
            await AddTransactions(card, fromTime);
        }

        public async Task AddTransactions(BankCardEntity card, long from, long? to = null)
        {
            to ??= DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var transactions = await SendRequest<List<TransactionEntity>>($"https://api.monobank.ua/personal/statement/0/{from}/{to}", card.Token);

            if (transactions == null || transactions.Count == 0)
                return;

            var existingIds = _dbContext.Transactions.Select(t => t.Id).ToHashSet();
            var newTransactions = transactions.Where(t => !existingIds.Contains(t.Id)).ToList();

            foreach (var transaction in newTransactions)
            {
                transaction.BankCardId = card.Id;
                _dbContext.Transactions.Add(transaction);
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task<T> SendRequest<T>(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Token");
            _httpClient.DefaultRequestHeaders.Add("X-Token", token);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }

    }
}