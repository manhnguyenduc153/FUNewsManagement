using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices
{
    public interface ISystemAccountService
    {
        Task<PagingResponse<SystemAccountDto>> GetListPagingAsync(SystemAccountSearchDto dto);
        Task<IEnumerable<SystemAccountDto>> GetAllAsync();
        Task<SystemAccountDto?> GetByIdAsync(short id);

        Task<ApiResponse<SystemAccountDto>> CreateOrEditAsync(SystemAccountSaveDto dto);
        Task<ApiResponse<bool>> DeleteAsync(short id);

        Task<ApiResponse<SystemAccountDto>> LoginAsync(SystemAccountLoginDto dto);
    }
}
