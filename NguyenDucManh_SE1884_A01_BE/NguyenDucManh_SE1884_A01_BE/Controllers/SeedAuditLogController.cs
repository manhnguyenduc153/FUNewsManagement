using Microsoft.AspNetCore.Mvc;
using NguyenDucManh_SE1884_A01_BE.Models;

namespace NguyenDucManh_SE1884_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedAuditLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedAuditLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedData()
        {
            _context.AuditLogs.Add(new AuditLog
            {
                User = "admin@FUNewsManagementSystem.org",
                Action = "CREATE",
                Entity = "Category",
                AfterData = "{\"CategoryId\":1,\"CategoryName\":\"Test\"}",
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return Ok("Seeded");
        }
    }
}
