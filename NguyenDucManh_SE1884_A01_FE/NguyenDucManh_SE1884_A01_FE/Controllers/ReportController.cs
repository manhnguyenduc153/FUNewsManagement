using Assignmen_PRN232__.Dto;
using Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;

        public ReportController(IReportService reportService, ICategoryService categoryService, ISystemAccountService systemAccountService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
        }

        public async Task<IActionResult> Index(ReportSearchDto dto)
        {
            var searchDto = new ReportSearchDto
            {
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
                Status = dto.Status,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate
            };

            var report = await _reportService.GetNewsArticleReportAsync(searchDto);

            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Authors = await _systemAccountService.GetAllAsync();

            ViewBag.CategoryId = searchDto.CategoryId;
            ViewBag.AuthorId = searchDto.AuthorId;
            ViewBag.Status = searchDto.Status;
            ViewBag.FromDate = searchDto.FromDate;
            ViewBag.ToDate = searchDto.ToDate;

            return View(report);
        }
    }
}
