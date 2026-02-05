namespace NguyenDucManh_SE1884_A01_BE.Dto;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class TokenResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = null!;
}
