using FinancialAssistent.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    public class MonobankBackgroundUpdater
    {
        private readonly MonobankUpdater _monobankUpdater;
        private readonly Database _dbContext;
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(1);

        public MonobankBackgroundUpdater(MonobankUpdater monobankUpdater, Database dbContext)
        {
            _monobankUpdater = monobankUpdater;
            _dbContext = dbContext;
        }

        public async Task StartUpdating(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var users = await _dbContext.BankCards.Select(c => new { c.UserId, c.Token }).ToListAsync();
                foreach (var user in users)
                {
                    await _monobankUpdater.UpdateUserData(user.UserId, user.Token);
                }

                await Task.Delay(_updateInterval, cancellationToken);
            }
        }
    }
}
