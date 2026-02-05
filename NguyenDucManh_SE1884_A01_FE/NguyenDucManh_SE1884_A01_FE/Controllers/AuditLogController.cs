using Assignmen_PRN232__.Dto;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class AuditLogController : Controller
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? entity = null, string? action = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var searchDto = new AuditLogSearchDto
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Entity = entity,
                    Action = action,
                    FromDate = fromDate,
                    ToDate = toDate
                };

                var result = await _auditLogService.GetListPagingAsync(searchDto);

                ViewBag.CurrentPage = searchDto.PageIndex;
                ViewBag.PageSize = searchDto.PageSize;
                ViewBag.Entity = searchDto.Entity;
                ViewBag.Action = searchDto.Action;
                ViewBag.FromDate = searchDto.FromDate;
                ViewBag.ToDate = searchDto.ToDate;
                ViewBag.TotalPages = result.TotalPages;
                ViewBag.TotalRecords = result.TotalRecords;

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new Assignmen_PRN232_1.DTOs.Common.PagingResponse<AuditLogDto>());
            }
        }
    }
}
