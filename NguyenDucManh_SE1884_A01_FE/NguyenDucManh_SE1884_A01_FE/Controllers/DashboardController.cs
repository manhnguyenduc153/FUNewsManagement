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

        public async Task<IActionResult> Index(AnalyticsFilterDto filter)
        {
            try
            {
                var dashboard = await _dashboardService.GetDashboardAsync(filter);
                var categories = await _dashboardService.GetCategoriesAsync();
                var authors = await _dashboardService.GetAuthorsAsync();
                
                ViewBag.Filter = filter;
                ViewBag.Categories = categories ?? new List<CategoryDto>();
                ViewBag.Authors = authors ?? new List<SystemAccountDto>();
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
                var categories = await _dashboardService.GetCategoriesAsync();
                var authors = await _dashboardService.GetAuthorsAsync();
                
                ViewBag.Filter = filter;
                ViewBag.Categories = categories ?? new List<CategoryDto>();
                ViewBag.Authors = authors ?? new List<SystemAccountDto>();
                return View(trending);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<TrendingArticleDto>());
            }
        }

        public async Task<IActionResult> Export(AnalyticsFilterDto filter)
        {
            try
            {
                var trending = await _dashboardService.GetTrendingAsync(filter);
                
                var csv = "NewsArticleId,NewsTitle,CategoryName,AuthorName,ViewCount,CreatedDate\n";
                foreach (var item in trending ?? new List<TrendingArticleDto>())
                {
                    csv += $"{item.NewsArticleId},\"{item.NewsTitle}\",\"{item.CategoryName}\",\"{item.AuthorName}\",{item.ViewCount},{item.CreatedDate:yyyy-MM-dd}\n";
                }
                
                var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
                return File(bytes, "text/csv", $"trending_{DateTime.Now:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Trending));
            }
        }
    }
}
