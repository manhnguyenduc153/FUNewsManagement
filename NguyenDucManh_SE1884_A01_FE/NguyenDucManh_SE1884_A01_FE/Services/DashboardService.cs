using Assignmen_PRN232__.Dto;

namespace Frontend.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto?> GetDashboardAsync(AnalyticsFilterDto? filter = null);
        Task<List<TrendingArticleDto>?> GetTrendingAsync(AnalyticsFilterDto filter);
        Task<List<CategoryDto>?> GetCategoriesAsync();
        Task<List<SystemAccountDto>?> GetAuthorsAsync();
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

        public async Task<DashboardDto?> GetDashboardAsync(AnalyticsFilterDto? filter = null)
        {
            var queryParams = new List<string>();
            if (filter?.FromDate.HasValue == true)
                queryParams.Add($"fromDate={filter.FromDate.Value:yyyy-MM-dd}");
            if (filter?.ToDate.HasValue == true)
                queryParams.Add($"toDate={filter.ToDate.Value:yyyy-MM-dd}");
            if (filter?.CategoryId.HasValue == true)
                queryParams.Add($"categoryId={filter.CategoryId}");
            if (filter?.AuthorId.HasValue == true)
                queryParams.Add($"authorId={filter.AuthorId}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"api/analytics/dashboard{query}");
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
            if (filter.Status.HasValue)
                queryParams.Add($"status={filter.Status}");

            var response = await _httpClient.GetAsync($"api/analytics/trending?{string.Join("&", queryParams)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TrendingArticleDto>>();
        }

        public async Task<List<CategoryDto>?> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/categories");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        }

        public async Task<List<SystemAccountDto>?> GetAuthorsAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/authors");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SystemAccountDto>>();
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
