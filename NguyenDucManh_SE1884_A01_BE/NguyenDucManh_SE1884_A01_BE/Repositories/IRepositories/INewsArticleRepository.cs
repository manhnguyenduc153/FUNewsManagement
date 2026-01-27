using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<PagingResponse<NewsArticle>> GetListPagingAsync(NewsArticleSearchDto searchDto);
        Task<NewsArticle?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;
        Task<NewsArticle?> GetByIdAsync(string id);
        Task<NewsArticle> AddAsync(NewsArticle newsArticle);
        Task UpdateAsync(NewsArticle newsArticle);
        Task DeleteAsync(NewsArticle newsArticle);
        Task<bool> ExistsByIdAsync(string newsArticleId);
        Task<int> SaveChangesAsync();
    }
}
