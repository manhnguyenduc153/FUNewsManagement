using NguyenDucManh_SE1884_A01_Analytic.Dto;

namespace NguyenDucManh_SE1884_A01_Analytic.Services
{
    public interface IAnalyticsService
    {
        Task<DashboardDto> GetDashboardAsync(string token);
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

        public async Task<DashboardDto> GetDashboardAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var baseUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";

            Console.WriteLine($"Calling: {baseUrl}/api/report/news-articles");
            var reportResponse = await client.GetAsync($"{baseUrl}/api/report/news-articles");
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
    }
}
