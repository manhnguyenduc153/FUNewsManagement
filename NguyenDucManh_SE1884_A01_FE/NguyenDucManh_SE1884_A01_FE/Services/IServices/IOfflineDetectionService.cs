namespace Frontend.Services.IServices
{
    public interface IOfflineDetectionService
    {
        bool IsOffline { get; }
        Task<bool> CheckApiConnectivityAsync();
    }
}
