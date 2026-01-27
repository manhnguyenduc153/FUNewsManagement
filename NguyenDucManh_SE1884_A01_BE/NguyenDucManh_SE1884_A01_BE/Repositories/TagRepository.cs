
using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories;
using NguyenDucManh_SE1884_A01_BE.Data;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace NguyenDucManh_SE1884_A01_BE.Repositories
{
    public class TagRepository
    : BaseRepository<Tag, AppDbContext>, ITagRepository
    {
        public TagRepository(AppDbContext context, IUnitOfWork unitOfWork)
            : base(context, unitOfWork)
        {
        }

        public async Task<PagingResponse<Tag>> GetListPagingAsync(TagSearchDto searchDto)
        {
            var query = FindAll(); 

            
            if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
            {
                var keyword = searchDto.Keyword.Trim();
                query = query.Where(x =>
                    x.TagName.Contains(keyword) ||
                    (x.Note != null && x.Note.Contains(keyword)));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.TagId)
                .Skip((searchDto.PageIndex - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return new PagingResponse<Tag>
            {
                PageIndex = searchDto.PageIndex,
                PageSize = searchDto.PageSize,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        public async Task<bool> ExistsByNameAsync(string tagName)
        {
            return await ExistsAsync(t => t.TagName == tagName);
        }

        public async Task<bool> ExistsByNameAsync(string tagName, int excludeTagId)
        {
            return await ExistsAsync(t => t.TagName == tagName && t.TagId != excludeTagId);
        }

        
        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Tag>()
                .Include(x => x.NewsArticles)
                .FirstOrDefaultAsync(x => x.TagId == id);
        }
    }

}
