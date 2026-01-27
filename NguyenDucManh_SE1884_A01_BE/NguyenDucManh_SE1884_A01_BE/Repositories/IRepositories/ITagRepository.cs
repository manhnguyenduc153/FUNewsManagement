using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<PagingResponse<Tag>> GetListPagingAsync(TagSearchDto searchDto);
        Task<Tag?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Tag tag);
        Task<bool> ExistsByNameAsync(string tagName);
        Task<bool> ExistsByNameAsync(string tagName, int excludeTagId);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<NewsArticle>> GetArticlesByTagAsync(int tagId);
    }

}
