using Frontend.Services.IServices;

namespace Frontend.Services
{
    public class OfflineDetectionService : IOfflineDetectionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private bool _isOffline = false;

        public OfflineDetectionService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public bool IsOffline => _isOffline;

        public async Task<bool> CheckApiConnectivityAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                
                var beApiUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";
                var response = await client.GetAsync($"{beApiUrl}/api/tags/all");
                
                _isOffline = !response.IsSuccessStatusCode;
                return !_isOffline;
            }
            catch
            {
                _isOffline = true;
                return false;
            }
        }
    }
}
