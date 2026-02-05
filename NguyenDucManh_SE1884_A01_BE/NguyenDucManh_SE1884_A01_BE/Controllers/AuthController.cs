using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenDucManh_SE1884_A01_BE.Common;
using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.Services.IServices;

namespace NguyenDucManh_SE1884_A01_BE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<TokenResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var account = await _context.SystemAccounts
            .FirstOrDefaultAsync(a => a.AccountEmail == request.Email && a.AccountPassword == request.Password);

        if (account == null)
        {
            return Ok(new ApiResponse<TokenResponseDto>
            {
                Success = false,
                StatusCode = 401,
                Message = "Invalid email or password"
            });
        }

        var accessToken = _jwtService.GenerateAccessToken(account);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var savedRefreshToken = await _jwtService.SaveRefreshTokenAsync(account.AccountId, refreshToken);

        return Ok(new ApiResponse<TokenResponseDto>
        {
            Success = true,
            StatusCode = 200,
            Message = "Login successful",
            Data = new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = savedRefreshToken.ExpiresAt
            }
        });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<TokenResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var refreshToken = await _jwtService.GetRefreshTokenAsync(request.RefreshToken);

        if (refreshToken == null)
        {
            return Ok(new ApiResponse<TokenResponseDto>
            {
                Success = false,
                StatusCode = 401,
                Message = "Invalid or expired refresh token"
            });
        }

        await _jwtService.RevokeRefreshTokenAsync(request.RefreshToken);

        var newAccessToken = _jwtService.GenerateAccessToken(refreshToken.Account);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var savedRefreshToken = await _jwtService.SaveRefreshTokenAsync(refreshToken.AccountId, newRefreshToken);

        return Ok(new ApiResponse<TokenResponseDto>
        {
            Success = true,
            StatusCode = 200,
            Message = "Token refreshed successfully",
            Data = new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = savedRefreshToken.ExpiresAt
            }
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<object>>> Logout([FromBody] RefreshTokenRequestDto request)
    {
        await _jwtService.RevokeRefreshTokenAsync(request.RefreshToken);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            StatusCode = 200,
            Message = "Logout successful"
        });
    }
}
