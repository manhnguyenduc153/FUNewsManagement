using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices
{
    public interface INewsArticleService
    {
        Task<PagingResponse<NewsArticleDto>> GetListPagingAsync(NewsArticleSearchDto dto);
        Task<PagingResponse<NewsArticleDto>> GetPublicListPagingAsync(NewsArticleSearchDto dto);
        Task<IEnumerable<NewsArticleDto>> GetAllAsync();
        Task<NewsArticleDto?> GetByIdAsync(string id);

        Task<ApiResponse<NewsArticleDto>> CreateOrEditAsync(NewsArticleSaveDto dto);
        Task<ApiResponse<NewsArticleDto>> DuplicateAsync(string id);
        Task<ApiResponse<bool>> DeleteAsync(string id);

        Task<ApiResponse<bool>> AddTagAsync(string newsArticleId, int tagId);
        Task<ApiResponse<bool>> RemoveTagAsync(string newsArticleId, int tagId);
        Task<IEnumerable<NewsArticleDto>> GetRelatedArticlesAsync(string newsArticleId);
    }
}
