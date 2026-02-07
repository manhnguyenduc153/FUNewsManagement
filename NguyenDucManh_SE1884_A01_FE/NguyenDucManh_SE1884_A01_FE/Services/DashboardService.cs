using Assignmen_PRN232__.Dto;

namespace Frontend.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto?> GetDashboardAsync();
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
    }
}
