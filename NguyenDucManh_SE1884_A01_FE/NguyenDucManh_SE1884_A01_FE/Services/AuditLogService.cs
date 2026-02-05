using Assignmen_PRN232__.Dto;
using Assignmen_PRN232_1.DTOs.Common;

namespace Frontend.Services
{
    public interface IAuditLogService
    {
        Task<PagingResponse<AuditLogDto>> GetListPagingAsync(AuditLogSearchDto dto);
    }

    public class AuditLogService : IAuditLogService
    {
        private readonly HttpClient _httpClient;

        public AuditLogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7053/");
        }

        public async Task<PagingResponse<AuditLogDto>> GetListPagingAsync(AuditLogSearchDto dto)
        {
            var queryParams = new List<string>
            {
                $"PageIndex={dto.PageIndex}",
                $"PageSize={dto.PageSize}"
            };

            if (!string.IsNullOrEmpty(dto.Entity))
                queryParams.Add($"Entity={dto.Entity}");

            if (dto.FromDate.HasValue)
                queryParams.Add($"FromDate={dto.FromDate.Value:yyyy-MM-dd}");

            if (dto.ToDate.HasValue)
                queryParams.Add($"ToDate={dto.ToDate.Value:yyyy-MM-dd}");

            var url = $"api/auditlog?{string.Join("&", queryParams)}";
            Console.WriteLine($"AuditLog Request URL: {_httpClient.BaseAddress}{url}");

            var response = await _httpClient.GetAsync(url);
            
            Console.WriteLine($"AuditLog Response Status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"AuditLog Response Content: {content}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagingResponse<AuditLogDto>>();
            return result ?? new PagingResponse<AuditLogDto>();
        }
    }
}
