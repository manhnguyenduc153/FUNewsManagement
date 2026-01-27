using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories
{
    public interface ISystemAccountRepository
    {
        Task<IEnumerable<SystemAccount>> GetAllAsync();
        Task<PagingResponse<SystemAccount>> GetListPagingAsync(SystemAccountSearchDto searchDto);
        Task<SystemAccount?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;
        Task<SystemAccount?> GetByIdAsync(short id);
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount> AddAsync(SystemAccount account);
        Task UpdateAsync(SystemAccount account);
        Task DeleteAsync(SystemAccount account);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email, short excludeAccountId);
        Task<int> SaveChangesAsync();
    }
}
