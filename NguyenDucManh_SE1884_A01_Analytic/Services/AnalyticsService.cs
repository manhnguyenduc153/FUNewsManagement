using NguyenDucManh_SE1884_A01_Analytics.Dto;

namespace NguyenDucManh_SE1884_A01_Analytics.Services
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

            var articlesResponse = await client.GetAsync($"{baseUrl}/api/newsarticles/all");
            articlesResponse.EnsureSuccessStatusCode();
            var articles = await articlesResponse.Content.ReadFromJsonAsync<List<NewsArticleDto>>();

            var categoriesResponse = await client.GetAsync($"{baseUrl}/api/categories/all");
            categoriesResponse.EnsureSuccessStatusCode();
            var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();

            return new DashboardDto
            {
                ArticlesByCategory = categories!.Select(c => new CategoryStatDto
                {
                    CategoryName = c.CategoryName!,
                    Count = articles!.Count(a => a.CategoryId == c.CategoryId)
                }).ToList(),

                ArticlesByStatus = new StatusStatDto
                {
                    ActiveCount = articles!.Count(a => a.NewsStatus == true),
                    InactiveCount = articles!.Count(a => a.NewsStatus == false)
                }
            };
        }
    }
}
