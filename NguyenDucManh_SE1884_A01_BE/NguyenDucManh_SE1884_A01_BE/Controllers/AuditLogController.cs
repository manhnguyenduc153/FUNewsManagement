using Assignmen_PRN232__.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;
using NguyenDucManh_SE1884_A01_BE.Models;

namespace NguyenDucManh_SE1884_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagingResponse<AuditLogDto>>> GetAuditLogs([FromQuery] AuditLogSearchDto dto)
        {
            var query = _context.AuditLogs.Where(x => x.Entity != "RefreshToken");

            if (!string.IsNullOrEmpty(dto.Entity))
                query = query.Where(x => x.Entity == dto.Entity);

            if (!string.IsNullOrEmpty(dto.Action))
                query = query.Where(x => x.Action == dto.Action);

            if (dto.FromDate.HasValue)
                query = query.Where(x => x.Timestamp >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(x => x.Timestamp <= dto.ToDate.Value);

            var totalRecords = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((dto.PageIndex - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(x => new AuditLogDto
                {
                    Id = x.Id,
                    User = x.User,
                    Action = x.Action,
                    Entity = x.Entity,
                    BeforeData = x.BeforeData,
                    AfterData = x.AfterData,
                    Timestamp = x.Timestamp
                })
                .ToListAsync();

            return new PagingResponse<AuditLogDto>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize
            };
        }
    }
}
