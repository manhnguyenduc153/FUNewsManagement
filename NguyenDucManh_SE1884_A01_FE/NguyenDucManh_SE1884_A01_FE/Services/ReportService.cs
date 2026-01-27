using Assignmen_PRN232__.Dto;
using Frontend.Services.IServices;

namespace Frontend.Services
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;

        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7053/api/");
        }

        public async Task<NewsArticleReportDto?> GetNewsArticleReportAsync(ReportSearchDto searchDto)
        {
            try
            {
                var queryParams = new List<string>();
                if (searchDto.CategoryId.HasValue && searchDto.CategoryId > 0)
                    queryParams.Add($"CategoryId={searchDto.CategoryId}");
                if (searchDto.AuthorId.HasValue && searchDto.AuthorId > 0)
                    queryParams.Add($"AuthorId={searchDto.AuthorId}");
                if (searchDto.Status.HasValue)
                    queryParams.Add($"Status={searchDto.Status}");
                if (searchDto.FromDate.HasValue)
                    queryParams.Add($"FromDate={searchDto.FromDate.Value:yyyy-MM-dd}");
                if (searchDto.ToDate.HasValue)
                    queryParams.Add($"ToDate={searchDto.ToDate.Value:yyyy-MM-dd}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

                var response = await _httpClient.GetAsync($"Report/news-articles{queryString}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var report = await response.Content.ReadFromJsonAsync<NewsArticleReportDto>();
                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNewsArticleReportAsync: {ex.Message}");
                return null;
            }
        }
    }
}
