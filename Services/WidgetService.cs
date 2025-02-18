using FinancialAssistent.Converters;
using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    [Route("api/WidgetService")]
    [ApiController]
    public class WidgetService
    {
        private readonly TransactionService transactionService;
        private readonly UserInfoService userInfoService;

        public WidgetService(TransactionService transactionService, UserInfoService userInfoService) 
        {
            this.transactionService = transactionService;
            this.userInfoService = userInfoService;
        }

        [HttpGet("getWidgets")]
        public List<Widgets> GetWidgets()
        {
            return transactionService._context.Widgets.Include(w => w.Icon).ToList();
        }

        [HttpPost("addStandartWidgets")]
        public async Task<List<Widgets>> AddStandartWidgets()
        {
            var categoryExpenses = GetCategoryExpences();

            var user = userInfoService.GetUser();

            List<Widgets> widgets = CreateDefaultWidgets(user.UserInfo.Id, categoryExpenses);

            await transactionService._context.Widgets.AddRangeAsync(widgets);
            await transactionService._context.SaveChangesAsync();
            return widgets;
        }

        [HttpPost("updateWidgets")]
        public async Task<IResult> UpdateWidgets([FromBody] WidgetUpdateModel[] model)
        {
            if (model == null || !model.Any())
            {
                return Results.BadRequest("Invalid widget data.");
            }
            var expences = GetCategoryExpences();

            var widgets = await transactionService._context.Widgets
                .Where(w => model.Select(m => m.Name).Contains(w.Name))
                .ToListAsync();

            foreach (var widgetModel in model)
            {
                var widget = widgets.FirstOrDefault(w => w.Name == widgetModel.Name);
                if (widget == null)
                {
                    return Results.NotFound($"Widget with name '{widgetModel.Name}' not found.");
                }

                widget.Budget = widgetModel.Budget;
                widget.Expenses = expences.FirstOrDefault(e => e.Key == widget.Name).Value;
            }

            try
            {
                await transactionService._context.SaveChangesAsync();
                return Results.Ok(new { message = "Widgets updated successfully" });
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private Dictionary<string, decimal> GetCategoryExpences()
        {
            DateTime today = DateTime.UtcNow;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var transactions = transactionService.GetTransactions(firstDayOfMonth, today);

            var categoryExpenses = transactions
                    .GroupBy(t => TransactionCategoryConverter.GetExpenseCategory(t.Mcc))
                    .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

            return categoryExpenses;
        }

        private List<Widgets> CreateDefaultWidgets(int userInfoId, Dictionary<string, decimal> categoryExpenses)
        {
            return new List<Widgets>
            {
                new Widgets { Name = "Mandatory Expenses", IconID = 1, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Mandatory Expenses", 0) },
                new Widgets { Name = "Food", IconID = 9, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Food", 0) },
                new Widgets { Name = "Sport and Health", IconID = 1, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Sport and Health", 0) },
                new Widgets { Name = "Transportation", IconID = 1, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Transportation", 0) },
                new Widgets { Name = "Entertainment", IconID = 1, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Entertainment", 0) }
            };
        }
    }
}
