using NguyenDucManh_SE1884_A01_Analytic.Dto;

namespace NguyenDucManh_SE1884_A01_Analytic.Services
{
    public interface IAnalyticsService
    {
        Task<DashboardDto> GetDashboardAsync(string token, AnalyticsFilterDto? filter = null);
        Task<List<TrendingArticleDto>> GetTrendingAsync(string token, AnalyticsFilterDto filter);
        Task<List<CategoryDto>> GetCategoriesAsync(string token);
        Task<List<SystemAccountDto>> GetAuthorsAsync(string token);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AnalyticsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<DashboardDto> GetDashboardAsync(string token, AnalyticsFilterDto? filter = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var baseUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";

            var queryParams = new List<string>();
            if (filter?.FromDate.HasValue == true)
                queryParams.Add($"FromDate={filter.FromDate.Value:yyyy-MM-dd}");
            if (filter?.ToDate.HasValue == true)
                queryParams.Add($"ToDate={filter.ToDate.Value:yyyy-MM-dd}");
            if (filter?.CategoryId.HasValue == true)
                queryParams.Add($"CategoryId={filter.CategoryId}");
            if (filter?.AuthorId.HasValue == true)
                queryParams.Add($"AuthorId={filter.AuthorId}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            Console.WriteLine($"Calling: {baseUrl}/api/report/news-articles{query}");
            var reportResponse = await client.GetAsync($"{baseUrl}/api/report/news-articles{query}");
            Console.WriteLine($"Report Response: {reportResponse.StatusCode}");
            reportResponse.EnsureSuccessStatusCode();
            
            var content = await reportResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Report Content: {content}");
            
            var report = await reportResponse.Content.ReadFromJsonAsync<NewsArticleReportDto>();

            return new DashboardDto
            {
                ArticlesByCategory = report!.CategoryStats.Select(c => new CategoryStatDto
                {
                    CategoryName = c.CategoryName,
                    Count = c.ArticleCount
                }).ToList(),

                ArticlesByStatus = new StatusStatDto
                {
                    ActiveCount = report.TotalActive,
                    InactiveCount = report.TotalInactive
                }
            };
        }

        public async Task<List<TrendingArticleDto>> GetTrendingAsync(string token, AnalyticsFilterDto filter)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var baseUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";

            var queryParams = new List<string>();
            if (filter.FromDate.HasValue)
                queryParams.Add($"FromDate={filter.FromDate.Value:yyyy-MM-dd}");
            if (filter.ToDate.HasValue)
                queryParams.Add($"ToDate={filter.ToDate.Value:yyyy-MM-dd}");
            if (filter.CategoryId.HasValue)
                queryParams.Add($"CategoryId={filter.CategoryId}");
            if (filter.AuthorId.HasValue)
                queryParams.Add($"AuthorId={filter.AuthorId}");
            if (filter.Status.HasValue)
                queryParams.Add($"Status={filter.Status}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            var articlesResponse = await client.GetAsync($"{baseUrl}/api/newsarticles/all{query}");
            articlesResponse.EnsureSuccessStatusCode();
            
            var articlesJson = await articlesResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {articlesJson.Substring(0, Math.Min(500, articlesJson.Length))}");
            
            var articles = await articlesResponse.Content.ReadFromJsonAsync<List<NewsArticleDto>>();

            if (filter.Status.HasValue)
            {
                articles = articles?.Where(a => a.NewsStatus == filter.Status).ToList();
            }

            Console.WriteLine($"Articles fetched: {articles?.Count}");
            if (articles != null && articles.Any())
            {
                Console.WriteLine($"First article: {articles[0].NewsTitle}, Category: {articles[0].CategoryName}, Author: {articles[0].CreatedByName}");
            }

            return articles!
                .OrderByDescending(a => a.ViewCount)
                .Take(filter.Top)
                .Select(a => new TrendingArticleDto
                {
                    NewsArticleId = a.NewsArticleId,
                    NewsTitle = a.NewsTitle ?? "N/A",
                    CategoryName = a.CategoryName ?? "N/A",
                    AuthorName = a.CreatedByName ?? "N/A",
                    ViewCount = a.ViewCount,
                    CreatedDate = a.CreatedDate
                })
                .ToList();
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var baseUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";
            var response = await client.GetAsync($"{baseUrl}/api/categories/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new();
        }

        public async Task<List<SystemAccountDto>> GetAuthorsAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var baseUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";
            var response = await client.GetAsync($"{baseUrl}/api/systemaccounts/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SystemAccountDto>>() ?? new();
        }
    }
}
