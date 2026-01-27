using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices
{
    public interface ICategoryService
    {
        Task<PagingResponse<CategoryDto>> GetListPagingAsync(CategorySearchDto dto);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(short id);

        Task<ApiResponse<CategoryDto>> CreateOrEditAsync(CategorySaveDto dto);
        Task<ApiResponse<bool>> DeleteAsync(short id);
    }
}
