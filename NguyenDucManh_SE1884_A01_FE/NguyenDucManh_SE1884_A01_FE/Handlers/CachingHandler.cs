using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Frontend.Handlers
{
    public class CachingHandler : DelegatingHandler
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachingHandler> _logger;
        private static readonly ConcurrentBag<string> _cacheKeys = new();

        public CachingHandler(IMemoryCache cache, ILogger<CachingHandler> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Clear all cache for POST/PUT/DELETE requests
            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put || request.Method == HttpMethod.Delete)
            {
                foreach (var key in _cacheKeys)
                {
                    _cache.Remove(key);
                }
                _cacheKeys.Clear();
                _logger.LogInformation("[Cache] Cleared all cache after {Method}", request.Method);
                return await base.SendAsync(request, cancellationToken);
            }

            // Only cache GET requests
            if (request.Method != HttpMethod.Get)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var cacheKey = $"http_cache_{request.RequestUri}";

            // Try to get from cache
            if (_cache.TryGetValue(cacheKey, out string? cachedResponse))
            {
                _logger.LogInformation("[Cache] Hit: {Url}", request.RequestUri);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(cachedResponse, System.Text.Encoding.UTF8, "application/json")
                };
            }

            // Call API
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    
                    // Cache for 5 minutes
                    _cache.Set(cacheKey, content, TimeSpan.FromMinutes(5));
                    _cacheKeys.Add(cacheKey);
                    _logger.LogInformation("[Cache] Stored: {Url}", request.RequestUri);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Cache] API call failed, checking cache");
                
                // If API fails, try cache again
                if (_cache.TryGetValue(cacheKey, out cachedResponse))
                {
                    _logger.LogInformation("[Cache] Fallback: {Url}", request.RequestUri);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(cachedResponse, System.Text.Encoding.UTF8, "application/json")
                    };
                }

                throw;
            }
        }
    }
}
