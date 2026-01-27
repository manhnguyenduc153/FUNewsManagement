namespace Frontend.Services.IServices
{
    public interface ILoginService
    {
        Task<(bool Success, string Message, string? Token)> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
