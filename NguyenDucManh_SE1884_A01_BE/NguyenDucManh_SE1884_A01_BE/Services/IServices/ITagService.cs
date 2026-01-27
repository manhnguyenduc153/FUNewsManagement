using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices
{
    public interface ITagService
    {
        Task<PagingResponse<TagDto>> GetListPagingAsync(TagSearchDto dto);
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto?> GetByIdAsync(int id);

        Task<ApiResponse<TagDto>> CreateOrEditAsync(TagSaveDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<IEnumerable<NewsArticleDto>> GetArticlesByTagAsync(int tagId);
    }
}
