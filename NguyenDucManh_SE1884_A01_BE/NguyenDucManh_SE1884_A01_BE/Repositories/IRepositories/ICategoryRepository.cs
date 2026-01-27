using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<PagingResponse<CategoryDto>> GetListPagingAsync(CategorySearchDto searchDto);
        Task<Category?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;
        Task<Category?> GetByIdAsync(short id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistsByNameAsync(string categoryName);
        Task<bool> ExistsByNameAndParentAsync(string categoryName, short? parentCategoryId, short excludeCategoryId = 0);
        Task<int> SaveChangesAsync();
    }
}
