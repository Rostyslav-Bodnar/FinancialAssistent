using FinancialAssistent.Services;
using Microsoft.AspNetCore.Mvc;

public class MonobankController : Controller
{
    private readonly WidgetService widgetService;
    private readonly UserInfoService userInfoService;
    private readonly BankCardService bankCardService;
    private readonly MonobankUpdater monobankUpdated;

    public MonobankController(WidgetService widgetService,
        UserInfoService userInfoService, BankCardService bankCardService, MonobankUpdater monobankUpdated)
    {
        this.widgetService = widgetService;
        this.userInfoService = userInfoService;
        this.bankCardService = bankCardService;
        this.monobankUpdated = monobankUpdated;
    }

    [HttpGet]
    public async Task<IActionResult> AddMonobankCard()
    {
        var user = userInfoService.GetUser();
        if (user == null) return RedirectToAction("Auth", "Auth");

        var bankCards = await bankCardService.GetBankCards(user.Id);

        if (bankCards != null && bankCards.Count != 0)
            return RedirectToAction("Dashboard", "FinancialAssistent");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddMonobankCard(string token)
    {
        var user = userInfoService.GetUser();
        Console.WriteLine("User added in AddCart");
        if (user == null) return RedirectToAction("Auth", "Auth");

        await monobankUpdated.CreateUserData(user.Id, token);
        await widgetService.AddStandartWidgets();

        return RedirectToAction("Dashboard", "FinancialAssistent");
    }
}
