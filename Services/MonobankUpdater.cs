using FinancialAssistent.Entities;
using FinancialAssistent.Models;

namespace FinancialAssistent.Services
{
    public class MonobankUpdater
    {
        private readonly MonobankService monobankService;
        private readonly Database dbContext;
        private readonly BankCardService bankCardService;
        private readonly TransactionService transactionService;

        public MonobankUpdater(MonobankService monobankService, Database dbContext, BankCardService bankCardService, TransactionService transactionService)
        {
            this.monobankService = monobankService;
            this.dbContext = dbContext;
            this.bankCardService = bankCardService;
            this.transactionService = transactionService;
        }

        public async Task CreateUserData(string userId, string token)
        {
            var account = await monobankService.GetAccountInfo(token);
            var bankCards = await bankCardService.CreateBankCards(userId, token, account);
            await UpdateData(bankCards, account);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserData(string userId, string token)
        {
            var account = await monobankService.GetAccountInfo(token);
            var bankCards = await bankCardService.GetBankCards(userId);
            
            await UpdateData(bankCards, account);

            await dbContext.SaveChangesAsync();
        }

        private async Task UpdateData(List<BankCardEntity> bankCards, MonobankAccountInfoModel account)
        {
            for (int i = 0; i < account.Accounts.Count; i++)
            {
                if (i < bankCards.Count)
                {
                    bankCards[i].Balance = account.Accounts[i].Balance / 100m;
                    bankCards[i].MaskedPan = account.Accounts[i].MaskedPan[0];
                    await transactionService.AddTransactions(bankCards[i], DateTimeOffset.UtcNow.AddMonths(-1).ToUnixTimeSeconds());
                }
            }
        }

    }
}
