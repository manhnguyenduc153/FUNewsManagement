using Assignmen_PRN232__.Dto;

namespace Frontend.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto?> GetDashboardAsync();
        Task<List<TrendingArticleDto>?> GetTrendingAsync(AnalyticsFilterDto filter);
        string GetExportUrl(AnalyticsFilterDto filter);
    }

    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7200/");
        }

        public async Task<DashboardDto?> GetDashboardAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/dashboard");
            response.EnsureSuccessStatusCode();
            
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            return await response.Content.ReadFromJsonAsync<DashboardDto>(options);
        }

        public async Task<List<TrendingArticleDto>?> GetTrendingAsync(AnalyticsFilterDto filter)
        {
            var queryParams = new List<string> { $"top={filter.Top}" };
            if (filter.FromDate.HasValue)
                queryParams.Add($"fromDate={filter.FromDate.Value:yyyy-MM-dd}");
            if (filter.ToDate.HasValue)
                queryParams.Add($"toDate={filter.ToDate.Value:yyyy-MM-dd}");
            if (filter.CategoryId.HasValue)
                queryParams.Add($"categoryId={filter.CategoryId}");
            if (filter.AuthorId.HasValue)
                queryParams.Add($"authorId={filter.AuthorId}");

            var response = await _httpClient.GetAsync($"api/analytics/trending?{string.Join("&", queryParams)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TrendingArticleDto>>();
        }

        public string GetExportUrl(AnalyticsFilterDto filter)
        {
            var queryParams = new List<string>();
            if (filter.FromDate.HasValue)
                queryParams.Add($"fromDate={filter.FromDate.Value:yyyy-MM-dd}");
            if (filter.ToDate.HasValue)
                queryParams.Add($"toDate={filter.ToDate.Value:yyyy-MM-dd}");
            if (filter.CategoryId.HasValue)
                queryParams.Add($"categoryId={filter.CategoryId}");
            if (filter.AuthorId.HasValue)
                queryParams.Add($"authorId={filter.AuthorId}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            return $"{_httpClient.BaseAddress}api/analytics/export{query}";
        }
    }
}
