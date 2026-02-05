using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices;

public interface IJwtService
{
    string GenerateAccessToken(SystemAccount account);
    string GenerateRefreshToken();
    Task<RefreshToken> SaveRefreshTokenAsync(short accountId, string token);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);
}
