# JWT Authentication API - Hướng dẫn sử dụng

## Tổng quan
Đã thêm JWT (JSON Web Token) authentication với Refresh Token cho FUNews Management API.

## Các API mới

### 1. Login (POST /api/auth/login)
Đăng nhập và nhận Access Token + Refresh Token

**Request:**
```json
{
  "email": "admin@FUNewsManagementSystem.org",
  "password": "@@123@@"
}
```

**Response:**
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Login successful",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64_encoded_token",
    "expiresAt": "2024-02-12T10:30:00Z"
  }
}
```

### 2. Refresh Token (POST /api/auth/refresh)
Làm mới Access Token khi hết hạn

**Request:**
```json
{
  "refreshToken": "base64_encoded_token"
}
```

**Response:**
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Token refreshed successfully",
  "data": {
    "accessToken": "new_access_token",
    "refreshToken": "new_refresh_token",
    "expiresAt": "2024-02-12T10:30:00Z"
  }
}
```

### 3. Logout (POST /api/auth/logout)
Thu hồi Refresh Token (cần Bearer token)

**Headers:**
```
Authorization: Bearer {access_token}
```

**Request:**
```json
{
  "refreshToken": "base64_encoded_token"
}
```

**Response:**
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Logout successful"
}
```

## Cấu hình JWT (appsettings.json)

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyForJWTTokenGeneration123456789",
    "Issuer": "FUNewsManagementAPI",
    "Audience": "FUNewsManagementClient",
    "AccessTokenExpirationMinutes": "30",
    "RefreshTokenExpirationDays": "7"
  }
}
```

## Sử dụng Bearer Token trong Swagger

1. Mở Swagger UI: `https://localhost:7053/swagger`
2. Click nút **Authorize** (ở góc trên bên phải)
3. Nhập: `Bearer {your_access_token}`
4. Click **Authorize**
5. Bây giờ tất cả API calls sẽ tự động gửi token

## Sử dụng Bearer Token trong Postman/HTTP Client

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Database Migration

Để tạo bảng RefreshToken trong database:

```bash
# Dừng ứng dụng trước
cd NguyenDucManh_SE1884_A01_BE\NguyenDucManh_SE1884_A01_BE

# Tạo migration
dotnet ef migrations add AddRefreshToken

# Update database
dotnet ef database update
```

## Cấu trúc bảng RefreshToken

```sql
CREATE TABLE RefreshToken (
    Id INT PRIMARY KEY IDENTITY,
    AccountId SMALLINT NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    IsRevoked BIT NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES SystemAccount(AccountID) ON DELETE CASCADE
)
```

## Luồng hoạt động

### 1. Login Flow
```
Client → POST /api/auth/login
       ← Access Token (30 phút) + Refresh Token (7 ngày)
```

### 2. API Call với Access Token
```
Client → GET /api/newsarticles
         Header: Authorization: Bearer {access_token}
       ← Data
```

### 3. Refresh Token Flow (khi Access Token hết hạn)
```
Client → POST /api/auth/refresh
         Body: { refreshToken: "..." }
       ← New Access Token + New Refresh Token
```

### 4. Logout Flow
```
Client → POST /api/auth/logout
         Header: Authorization: Bearer {access_token}
         Body: { refreshToken: "..." }
       ← Success (Refresh Token bị thu hồi)
```

## Token Claims

Access Token chứa các claims:
- `NameIdentifier`: AccountId
- `Email`: Account Email
- `Name`: Account Name
- `Role`: Account Role (0=Admin, 1=Staff, 2=Lecturer)

## Bảo mật

✅ Access Token có thời gian sống ngắn (30 phút)
✅ Refresh Token có thời gian sống dài (7 ngày)
✅ Refresh Token được lưu trong database và có thể thu hồi
✅ Mỗi lần refresh sẽ tạo token mới và thu hồi token cũ
✅ JWT được ký bằng HMAC-SHA256

## Lưu ý

⚠️ **Quan trọng:** Thay đổi `Jwt:Key` trong production thành chuỗi ngẫu nhiên phức tạp
⚠️ Không lưu Access Token trong localStorage (dễ bị XSS attack)
⚠️ Nên lưu Refresh Token trong httpOnly cookie
⚠️ Trong production, nên hash password trước khi lưu database

## Test Accounts

```
Admin:
- Email: admin@FUNewsManagementSystem.org
- Password: @@123@@

Staff:
- Email: IsabellaDavid@FUNewsManagement.org
- Password: @1

Lecturer:
- Email: OliviaJames@FUNewsManagement.org
- Password: @1
```

## Files đã thêm/sửa

### Files mới:
- `Models/RefreshToken.cs` - Entity model
- `Dto/AuthDto.cs` - DTOs cho authentication
- `Services/IJwtService.cs` - Interface
- `Services/JwtService.cs` - JWT service implementation
- `Controllers/AuthController.cs` - Auth endpoints

### Files đã sửa:
- `Program.cs` - Thêm JWT authentication + Swagger Bearer config
- `Data/AppDbContext.cs` - Thêm RefreshTokens DbSet
- `appsettings.json` - Thêm JWT configuration
- `Extensions/ServicesRegister.cs` - Register JWT service

## Troubleshooting

**Lỗi: "Invalid object name 'RefreshToken'"**
→ Chạy migration: `dotnet ef database update`

**Lỗi: "401 Unauthorized"**
→ Kiểm tra token đã hết hạn chưa, dùng refresh token để lấy token mới

**Lỗi: "The process cannot access the file"**
→ Dừng ứng dụng đang chạy trước khi build
