using FinancialAssistent.Services;
using Microsoft.AspNetCore.Mvc;

public class MonobankController : Controller
{
    private readonly MonobankUpdater monobankUpdater;
    private readonly WidgetService widgetService;
    private readonly UserInfoService userInfoService;

    public MonobankController(MonobankUpdater monobankUpdater, WidgetService widgetService,
        UserInfoService userInfoService)
    {
        this.monobankUpdater = monobankUpdater;
        this.widgetService = widgetService;
        this.userInfoService = userInfoService;
    }

    [HttpGet]
    public IActionResult AddMonobankCard()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddMonobankCard(string token)
    {
        var user = userInfoService.GetUser();
        if (user == null) return RedirectToAction("Auth", "Auth");

        await monobankUpdater.UpdateUserData(user.Id, token);
        await widgetService.AddStandartWidgets();

        return RedirectToAction("Dashboard", "FinanicalAssistent");
    }
}
