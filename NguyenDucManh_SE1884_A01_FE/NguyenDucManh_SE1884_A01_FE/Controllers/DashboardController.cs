using Assignmen_PRN232__.Dto;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboard = await _dashboardService.GetDashboardAsync();
                return View(dashboard);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new Assignmen_PRN232__.Dto.DashboardDto());
            }
        }

        public async Task<IActionResult> Trending(AnalyticsFilterDto filter)
        {
            try
            {
                filter.Top = filter.Top > 0 ? filter.Top : 10;
                var trending = await _dashboardService.GetTrendingAsync(filter);
                ViewBag.Filter = filter;
                ViewBag.ExportUrl = _dashboardService.GetExportUrl(filter);
                return View(trending);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<TrendingArticleDto>());
            }
        }
    }
}
