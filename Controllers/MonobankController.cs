using FinancialAssistent.Entities;
using FinancialAssistent.Models;
using FinancialAssistent.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

public class MonobankController : Controller
{
    private readonly MonobankService _monobankService;
    private readonly UserManager<User> _userManager;
    private readonly Database _dbContext;

    public MonobankController(MonobankService monobankService, UserManager<User> userManager, Database dbContext)
    {
        _monobankService = monobankService;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Auth", "Auth");

        var bankCard = await _dbContext.BankCards
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);

        if (bankCard == null) return RedirectToAction("AddMonobankCard");

        return View(new DashboardViewModel
        {
            User = user,
            Balance = bankCard.Balance,
            TotalBalance = bankCard.Balance + user.Cash,
            HasCard = true,
            Transactions = bankCard.Transactions.OrderByDescending(t => t.Time).ToList(),
            MonthBudget = 0
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

        await _monobankService.UpdateCardInfo(user.Id, token);

        return RedirectToAction("Dashboard");
    }
}
