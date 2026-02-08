using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Frontend.Controllers
{
    public class CacheStatusController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheStatusController> _logger;

        public CacheStatusController(IMemoryCache cache, ILogger<CacheStatusController> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        // GET: /CacheStatus
        public IActionResult Index()
        {
            var cacheStats = new
            {
                Message = "Cache is active. Check server logs for background service activity.",
                Instructions = new[]
                {
                    "Background service runs every 30 seconds",
                    "Check console logs for: [Background] Starting data cache refresh",
                    "Check console logs for: [Cache] Hit: ... (when using cached data)",
                    "Check console logs for: [Cache] Stored: ... (when caching new data)"
                }
            };

            return Json(cacheStats);
        }

        // GET: /CacheStatus/Clear
        public IActionResult Clear()
        {
            // Note: IMemoryCache doesn't have a clear all method
            // This is just a placeholder
            _logger.LogWarning("[Cache] Clear requested - restart app to clear all cache");
            
            return Json(new { message = "Cache clear requested. Restart app to fully clear cache." });
        }
    }
}
