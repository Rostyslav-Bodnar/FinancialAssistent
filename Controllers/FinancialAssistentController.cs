using FinancialAssistent.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Controllers
{
    public class FinancialAssistentController : Controller
    {
        private readonly DashboardModelGenerator dashboardModelGenerator;
        private readonly ProfileInfoModelGenerator profileInfoModelGenerator;
        public FinancialAssistentController(DashboardModelGenerator dashboardModelGenerator,
            ProfileInfoModelGenerator profileInfoModelGenerator)
        {
            this.dashboardModelGenerator = dashboardModelGenerator;
            this.profileInfoModelGenerator = profileInfoModelGenerator;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await dashboardModelGenerator.GenerateDashboardModel();
            if (model == null) return RedirectToAction("Auth", "Auth");
            return View(model);
        }

        public async Task<IActionResult> ProfileSettings()
        {
            var model = await profileInfoModelGenerator.GenerateModel();
            if (model == null) return RedirectToAction("Auth", "Auth");
            return View(model);
        }

    }
}
