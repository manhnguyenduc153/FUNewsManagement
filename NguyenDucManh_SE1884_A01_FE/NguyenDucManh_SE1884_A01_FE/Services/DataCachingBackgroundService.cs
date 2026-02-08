using Frontend.Services.IServices;

namespace Frontend.Services
{
    public class DataCachingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataCachingBackgroundService> _logger;

        public DataCachingBackgroundService(IServiceProvider serviceProvider, ILogger<DataCachingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[Background] Data caching service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CacheDataAsync();
                    
                    // Run every 30 seconds for testing (change to 5 minutes in production)
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Background] Error caching data");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task CacheDataAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            
            var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
            var tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
            var newsArticleService = scope.ServiceProvider.GetRequiredService<INewsArticleService>();

            _logger.LogInformation("[Background] Starting data cache refresh");

            try
            {
                // Cache categories
                await categoryService.GetAllAsync();
                _logger.LogInformation("[Background] Cached categories");

                // Cache tags
                await tagService.GetAllAsync();
                _logger.LogInformation("[Background] Cached tags");

                // Cache news articles (first page)
                await newsArticleService.GetListPagingAsync(new Assignmen_PRN232__.Dto.NewsArticleSearchDto
                {
                    PageIndex = 1,
                    PageSize = 20
                });
                _logger.LogInformation("[Background] Cached news articles");

                _logger.LogInformation("[Background] Data cache refresh completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Background] Failed to cache data");
            }
        }
    }
}
