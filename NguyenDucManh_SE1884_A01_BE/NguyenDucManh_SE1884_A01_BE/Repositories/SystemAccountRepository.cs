using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories;
using NguyenDucManh_SE1884_A01_BE.Data;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace NguyenDucManh_SE1884_A01_BE.Repositories
{
    public class SystemAccountRepository
    : BaseRepository<SystemAccount, AppDbContext>, ISystemAccountRepository
    {
        public SystemAccountRepository(AppDbContext context, IUnitOfWork unitOfWork)
            : base(context, unitOfWork)
        {
        }

        public async Task<PagingResponse<SystemAccount>> GetListPagingAsync(SystemAccountSearchDto searchDto)
        {
            var query = FindAll();

            
            if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
            {
                var keyword = searchDto.Keyword.Trim();
                query = query.Where(x =>
                    x.AccountName.Contains(keyword) ||
                    x.AccountEmail.Contains(keyword));
            }

            
            if (searchDto.AccountRole.HasValue && searchDto.AccountRole >= 0)
            {
                query = query.Where(x => x.AccountRole == searchDto.AccountRole);
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.AccountId)
                .Skip((searchDto.PageIndex - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return new PagingResponse<SystemAccount>
            {
                PageIndex = searchDto.PageIndex,
                PageSize = searchDto.PageSize,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await FindAll()
                .FirstOrDefaultAsync(x => x.AccountEmail == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await ExistsAsync(x => x.AccountEmail == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email, short excludeAccountId)
        {
            return await ExistsAsync(x => x.AccountEmail == email && x.AccountId != excludeAccountId);
        }

        
        public Task<SystemAccount?> GetByIdAsync(short id) => GetByIdAsync<short>(id);
    }
}
