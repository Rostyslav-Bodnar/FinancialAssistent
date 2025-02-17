using FinancialAssistent.Entities;
using FinancialAssistent.Migrations;
using FinancialAssistent.Models;
using FinancialAssistent.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MonobankController : Controller
{
    private readonly MonobankService _monobankService;
    private readonly WidgetService _widgetService;
    private readonly UserManager<User> _userManager;
    private readonly Database _dbContext;

    public MonobankController(MonobankService monobankService, UserManager<User> userManager, 
        Database dbContext, WidgetService widgetService)
    {
        _monobankService = monobankService;
        _userManager = userManager;
        _dbContext = dbContext;
        _widgetService = widgetService;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Auth", "Auth");

        var bankCard = await _dbContext.BankCards
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);

        if (bankCard == null) return RedirectToAction("AddMonobankCard");

        var transactionService = new TransactionService(_dbContext, _userManager, new HttpContextAccessor());
        var monthlyBudgetService = new MonthlyBudgetService(transactionService);
        var budgetInfo = monthlyBudgetService.GetMonthlyBudgetInfo();

        var CostLimit = new CostLimitsService(transactionService);
        var costInfo = CostLimit.GetCostsLimits();
        //_widgetService.AddStandartWidgets();
        return View(new DashboardViewModel
        {
            User = user,
            Balance = bankCard.Balance,
            TotalBalance = bankCard.Balance + user.UserInfo.Cash,
            HasCard = true,
            Transactions = bankCard.Transactions.OrderByDescending(t => t.Time).ToList(),
            monthlyBudgetModel = budgetInfo,
            costLimitsModel = costInfo,
            widgets = _widgetService.GetWidgets(),
            icons = _dbContext.Icons.ToList(),
        });
    }

    [HttpGet]
    public IActionResult AddMonobankCard()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddMonobankCard(string token)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Auth", "Auth");

        await _monobankService.UpdateCardInfo(user.Id, "uct8caooBtX8aNC6mQYCmji_4XABpYGCcKuVWc0ZSkH0");

        return RedirectToAction("Dashboard");
    }

}
