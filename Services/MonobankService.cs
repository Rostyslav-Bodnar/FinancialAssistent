using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;


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
            _httpClient.DefaultRequestHeaders.Add("X-Token", token);
            HttpResponseMessage response = await _httpClient.GetAsync("https://api.monobank.ua/personal/client-info");
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MonobankAccountInfoModel>(jsonResponse);

            /*_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Token", token);

            HttpResponseMessage response = await _httpClient.GetAsync("https://api.monobank.ua/personal/client-info");
            Console.WriteLine($"Response Code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorResponse}");
                throw new HttpRequestException($"Monobank API error: {response.StatusCode}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MonobankAccountInfoModel>(jsonResponse);*/
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

        public async Task UpdateCardInfo(string userId, string token)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("X-Token"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Token", token);
            }


            // Отримання інформації про баланс
            var response = await _httpClient.GetAsync("https://api.monobank.ua/personal/client-info");
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var accountInfo = JsonConvert.DeserializeObject<MonobankAccountInfoModel>(jsonResponse);
            var bankCard = await GetOrCreateBankCard(userId, token);
            bankCard.Balance = accountInfo.Accounts[0].Balance / 100m;

            await _dbContext.SaveChangesAsync();
            await AddTransactions(bankCard);
        }

        public async Task AddTransactions(BankCardEntity card)
        {
            //long fromTime = DateTimeOffset.UtcNow.AddMonths(-1).ToUnixTimeSeconds();\
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Поточний час у UNIX
            long fromTime = currentTime - 2682000;
            var transactionsResponse = await _httpClient.GetAsync($"https://api.monobank.ua/personal/statement/0/{fromTime}/{currentTime}");
            transactionsResponse.EnsureSuccessStatusCode();
            string transactionsJson = await transactionsResponse.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<List<TransactionEntity>>(transactionsJson);
            if(transactions.Count == 0)
            {
                Console.WriteLine("Poor boy");
            }
            foreach (var transaction in transactions)
            {
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (!_dbContext.Transactions.Any(t => t.Id == transaction.Id))
                {
                    transaction.BankCardId = card.Id;
                    _dbContext.Transactions.Add(transaction);
                }
                else
                {
                    Console.WriteLine("Bad");
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddTransactions(BankCardEntity card, long from, long? to = null)
        {
            if(to == null)
            {
                to = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            var transactionsResponse = await _httpClient.GetAsync($"https://api.monobank.ua/personal/statement/0/{from}");
            Console.WriteLine(transactionsResponse.EnsureSuccessStatusCode().RequestMessage);
            transactionsResponse.EnsureSuccessStatusCode();
            string transactionsJson = await transactionsResponse.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<List<TransactionEntity>>(transactionsJson);
            if (transactions.Count == 0)
            {
                Console.WriteLine("Poor boy");
            }
            foreach (var transaction in transactions)
            {
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (!_dbContext.Transactions.Any(t => t.Id == transaction.Id))
                {
                    transaction.BankCardId = card.Id;
                    _dbContext.Transactions.Add(transaction);
                }
                else
                {
                    Console.WriteLine("Bad");
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TransactionEntity>> GetTransactions(BankCardEntity card)
        {
            var transactions = _dbContext.Transactions.Where(t => card.Id == t.BankCardId).ToList();
            if (transactions == null) return null;
            return transactions;
        }

    }
}