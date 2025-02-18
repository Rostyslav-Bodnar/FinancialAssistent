using FinancialAssistent.Entities;
using FinancialAssistent.Helpers;
using FinancialAssistent.Services;
using FinancialAssistent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Controllers
{
    public class FinancialAssistentController : Controller
    {
        private readonly DashboardModelGenerator dashboardModelGenerator;

        public FinancialAssistentController(DashboardModelGenerator dashboardModelGenerator)
        {
            this.dashboardModelGenerator = dashboardModelGenerator;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await dashboardModelGenerator.GenerateDashboardModel();
            if (model == null) return RedirectToAction("Auth", "Auth");

            return View(model);
        }
    }
}
