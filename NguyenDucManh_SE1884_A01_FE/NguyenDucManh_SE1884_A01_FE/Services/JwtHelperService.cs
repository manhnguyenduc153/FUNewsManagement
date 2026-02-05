using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend.Services
{
    public interface IJwtHelperService
    {
        (bool IsAuthenticated, string? UserName, string? Email, string? Role) GetUserInfo();
    }

    public class JwtHelperService : IJwtHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (bool IsAuthenticated, string? UserName, string? Email, string? Role) GetUserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return (false, null, null, null);

            var token = httpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
                return (false, null, null, null);

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                return (true, userName, email, role);
            }
            catch
            {
                return (false, null, null, null);
            }
        }
    }
}
