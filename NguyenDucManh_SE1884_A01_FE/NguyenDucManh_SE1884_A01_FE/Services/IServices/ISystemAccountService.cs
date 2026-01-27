using Assignmen_PRN232__.Dto;
using Assignmen_PRN232_1.DTOs.Common;

namespace Frontend.Services.IServices
{
    public interface ISystemAccountService
    {
        Task<PagingResponse<SystemAccountDto>> GetListPagingAsync(SystemAccountSearchDto searchDto);
        Task<List<SystemAccountDto>> GetAllAsync();
        Task<SystemAccountDto?> GetByIdAsync(short id);
        Task<(bool Success, string Message, SystemAccountDto? Data)> CreateOrEditAsync(SystemAccountSaveDto dto);
        Task<(bool Success, string Message)> DeleteAsync(short id);
    }
}
