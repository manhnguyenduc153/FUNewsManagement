using Assignmen_PRN232__.Dto;
using Assignmen_PRN232_1.DTOs.Common;

namespace Frontend.Services.IServices
{
    public interface ICategoryService
    {
        Task<PagingResponse<CategoryDto>> GetListPagingAsync(CategorySearchDto searchDto);
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message, CategoryDto? Data)> CreateOrEditAsync(CategorySaveDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
