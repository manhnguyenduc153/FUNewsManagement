using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories;
using NguyenDucManh_SE1884_A01_BE.Data;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace NguyenDucManh_SE1884_A01_BE.Repositories
{
    public class NewsArticleRepository
    : BaseRepository<NewsArticle, AppDbContext>, INewsArticleRepository
    {
        public NewsArticleRepository(AppDbContext context, IUnitOfWork unitOfWork)
            : base(context, unitOfWork)
        {
        }

        public async Task<PagingResponse<NewsArticle>> GetListPagingAsync(NewsArticleSearchDto searchDto)
        {
            var query = _dbContext.Set<NewsArticle>()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .AsNoTracking()
                .Where(x => x.Category != null && x.Category.IsActive.Value);


            if (!string.IsNullOrWhiteSpace(searchDto.Title))
            {
                query = query.Where(x => x.NewsTitle.Contains(searchDto.Title));
            }

            
            if (!string.IsNullOrWhiteSpace(searchDto.Author))
            {
                query = query.Where(x => x.CreatedBy != null && x.CreatedBy.AccountName.Contains(searchDto.Author));
            }

            
            if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
            {
                var keyword = searchDto.Keyword.Trim();
                query = query.Where(x =>
                    x.NewsTitle.Contains(keyword) ||
                    x.Headline.Contains(keyword) ||
                    (x.NewsContent != null && x.NewsContent.Contains(keyword)));
            }

            
            if (searchDto.CategoryId.HasValue && searchDto.CategoryId > 0)
            {
                query = query.Where(x => x.CategoryId == searchDto.CategoryId);
            }

            
            if (searchDto.Status.HasValue)
            {
                query = query.Where(x => x.NewsStatus == searchDto.Status);
            }

            
            if (searchDto.FromDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= searchDto.FromDate);
            }
            if (searchDto.ToDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= searchDto.ToDate);
            }

            
            if (searchDto.CreatedById.HasValue && searchDto.CreatedById > 0)
            {
                query = query.Where(x => x.CreatedById == searchDto.CreatedById);
            }

            var totalRecords = await query.CountAsync();

            
            var items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((searchDto.PageIndex - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return new PagingResponse<NewsArticle>
            {
                PageIndex = searchDto.PageIndex,
                PageSize = searchDto.PageSize,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        public async Task<bool> ExistsByIdAsync(string newsArticleId)
        {
            return await ExistsAsync(x => x.NewsArticleId == newsArticleId);
        }

        
        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<NewsArticle>()
                .Include(x => x.Tags)
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.NewsArticleId == id);
        }

        public async Task<IEnumerable<NewsArticle>> GetRelatedArticlesAsync(string newsArticleId)
        {
            var currentArticle = await GetByIdAsync(newsArticleId);
            if (currentArticle == null)
                return new List<NewsArticle>();

            var relatedArticles = await _dbContext.Set<NewsArticle>()
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .Where(x => x.NewsArticleId != newsArticleId && x.NewsStatus == true)
                .Where(x => x.CategoryId == currentArticle.CategoryId || 
                            x.Tags.Any(t => currentArticle.Tags.Select(ct => ct.TagId).Contains(t.TagId)))
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .ToListAsync();

            return relatedArticles;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _dbContext.Set<NewsArticle>()
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Tags)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
