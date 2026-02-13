using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NguyenDucManh_SE1884_A01_Analytic.Dto;
using NguyenDucManh_SE1884_A01_Analytic.Services;

namespace NguyenDucManh_SE1884_A01_Analytic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardDto>> GetDashboard([FromQuery] AnalyticsFilterDto? filter)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _analyticsService.GetDashboardAsync(token, filter);
            return Ok(result);
        }

        [HttpGet("trending")]
        public async Task<ActionResult<List<TrendingArticleDto>>> GetTrending([FromQuery] AnalyticsFilterDto filter)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _analyticsService.GetTrendingAsync(token, filter);
            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel([FromQuery] AnalyticsFilterDto filter)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var data = await _analyticsService.GetTrendingAsync(token, filter);
            
            var csv = "NewsArticleId,NewsTitle,CategoryName,AuthorName,ViewCount,CreatedDate\n";
            foreach (var item in data)
            {
                csv += $"{item.NewsArticleId},\"{item.NewsTitle}\",\"{item.CategoryName}\",\"{item.AuthorName}\",{item.ViewCount},{item.CreatedDate:yyyy-MM-dd}\n";
            }
            
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"trending_{DateTime.Now:yyyyMMdd}.csv");
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _analyticsService.GetCategoriesAsync(token);
            return Ok(result);
        }

        [HttpGet("authors")]
        public async Task<ActionResult<List<SystemAccountDto>>> GetAuthors()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _analyticsService.GetAuthorsAsync(token);
            return Ok(result);
        }
    }
}
