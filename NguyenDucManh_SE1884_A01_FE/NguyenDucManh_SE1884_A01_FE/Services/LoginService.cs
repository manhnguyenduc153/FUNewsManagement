using Assignmen_PRN232__.Dto;
using Assignmen_PRN232__.Dto.Common;
using Assignmen_PRN232__.Enums;
using Frontend.Services.IServices;

namespace Frontend.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7053/api/");
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClientHandler CreateHandlerWithCookies()
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();
            handler.UseCookies = true;
            return handler;
        }

        public async Task<(bool Success, string Message, string? Token)> LoginAsync(string email, string password)
        {
            try
            {
                var loginDto = new { email = email, password = password };
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    return (false, "Invalid email or password", null);
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResponseDto>>();

                if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                {
                    return (false, apiResponse?.Message ?? "Login failed", null);
                }

                var token = apiResponse.Data.AccessToken;
                var refreshToken = apiResponse.Data.RefreshToken;

                // Save tokens to session
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Session.SetString("AccessToken", token);
                    httpContext.Session.SetString("RefreshToken", refreshToken);
                    httpContext.Session.SetString("IsAuthenticated", "true");
                }

                return (true, apiResponse.Message ?? "Login successful", token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var refreshToken = httpContext.Session.GetString("RefreshToken");
                    var accessToken = httpContext.Session.GetString("AccessToken");

                    if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(accessToken))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                        await _httpClient.PostAsJsonAsync("auth/logout", new { refreshToken });
                    }

                    httpContext.Session.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogoutAsync: {ex.Message}");
            }
        }
    }
}
