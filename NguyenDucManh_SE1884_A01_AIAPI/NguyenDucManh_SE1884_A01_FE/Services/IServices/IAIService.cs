using NguyenDucManh_SE1884_A01_AIAPI.Dto;

namespace NguyenDucManh_SE1884_A01_AIAPI.Services.IServices
{
    public interface IAIService
    {
        Task<SuggestTagsResponse> SuggestTagsAsync(string content);
    }
}
