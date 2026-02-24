using Frontend.Services.IServices;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIService> _logger;
        private const string AI_API_BASE_URL = "https://localhost:7300";

        public AIService(HttpClient httpClient, ILogger<AIService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(AI_API_BASE_URL);
            _logger = logger;
        }

        public async Task<AISuggestTagsResponse> SuggestTagsAsync(string content)
        {
            _logger.LogInformation("[FE-AIService] SuggestTagsAsync called with content length: {Length}", content.Length);
            
            try
            {
                var request = new { Content = content };
                var json = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("[FE-AIService] Calling {Url}/api/ai/suggest-tags", AI_API_BASE_URL);
                var response = await _httpClient.PostAsync("/api/ai/suggest-tags", httpContent);

                _logger.LogInformation("[FE-AIService] Response status: {Status}", response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[FE-AIService] Response JSON: {Json}", responseJson);
                    
                    return JsonSerializer.Deserialize<AISuggestTagsResponse>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new AISuggestTagsResponse();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("[FE-AIService] API call failed: {Status}, {Error}", response.StatusCode, errorContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FE-AIService] Exception calling AI API");
            }

            return new AISuggestTagsResponse();
        }
    }
}
