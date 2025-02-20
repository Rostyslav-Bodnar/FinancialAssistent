using FinancialAssistent.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    public class MonobankBackgroundUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MonobankBackgroundUpdater> _logger;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(60);

        public MonobankBackgroundUpdater(IServiceScopeFactory serviceScopeFactory, ILogger<MonobankBackgroundUpdater> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Monobank background updater запущено.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<Database>();
                        var monobankUpdater = scope.ServiceProvider.GetRequiredService<MonobankUpdater>();

                        var usersWithCards = await dbContext.BankCards
                            .Include(c => c.User)
                            .Where(c => !string.IsNullOrEmpty(c.Token))
                            .GroupBy(c => c.User)
                            .ToListAsync(stoppingToken);

                        foreach (var userGroup in usersWithCards)
                        {
                            var user = userGroup.Key; // Отримуємо користувача
                            foreach (var bankCard in userGroup)
                            {
                                _logger.LogInformation($"Оновлення даних користувача {user.Id} для карти {bankCard.Id}");
                                //await Task.Delay(600);
                                //await monobankUpdater.UpdateUserData(user.Id, bankCard.Token);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка під час оновлення даних користувачів.");
                }

                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

    }
}
