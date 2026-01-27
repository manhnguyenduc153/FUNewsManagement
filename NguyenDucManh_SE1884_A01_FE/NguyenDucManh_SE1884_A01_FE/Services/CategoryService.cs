using Assignmen_PRN232__.Dto;
using Assignmen_PRN232__.Dto.Common;
using Assignmen_PRN232_1.DTOs.Common;
using Frontend.Services.IServices;

namespace Frontend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Lấy danh sách phân trang
        public async Task<PagingResponse<CategoryDto>> GetListPagingAsync(CategorySearchDto searchDto)
        {
            try
            {
                // Build query string
                var queryParams = new List<string>();
                if (searchDto.PageIndex > 0)
                    queryParams.Add($"PageIndex={searchDto.PageIndex}");
                if (searchDto.PageSize > 0)
                    queryParams.Add($"PageSize={searchDto.PageSize}");
                if (!string.IsNullOrEmpty(searchDto.Keyword))
                    queryParams.Add($"Keyword={Uri.EscapeDataString(searchDto.Keyword)}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

                var response = await _httpClient.GetAsync($"https://localhost:7053/api/Categories{queryString}");

                if (!response.IsSuccessStatusCode)
                {
                    return new PagingResponse<CategoryDto>();
                }

                var pagingResponse = await response.Content
                    .ReadFromJsonAsync<PagingResponse<CategoryDto>>();

                return pagingResponse ?? new PagingResponse<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetListPagingAsync: {ex.Message}");
                return new PagingResponse<CategoryDto>();
            }
        }

        // GET: Lấy tất cả (không phân trang) - nếu cần
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            try
            {
                var searchDto = new CategorySearchDto
                {
                    PageIndex = 1,
                    PageSize = 1000 // Lấy nhiều để có tất cả
                };

                var result = await GetListPagingAsync(searchDto);
                return result.Items?.ToList() ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                return new List<CategoryDto>();
            }
        }

        // GET: Lấy theo ID
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7053/api/Categories/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var category = await response.Content
                    .ReadFromJsonAsync<CategoryDto>();

                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                return null;
            }
        }

        // POST: Tạo mới hoặc cập nhật
        public async Task<(bool Success, string Message, CategoryDto? Data)> CreateOrEditAsync(CategorySaveDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7053/api/Categories/create-or-edit", dto);

                var apiResponse = await response.Content
                    .ReadFromJsonAsync<ApiResponse<CategoryDto>>();

                if (apiResponse == null)
                {
                    return (false, "Failed to process response", null);
                }

                return (apiResponse.Success, apiResponse.Message ?? "", apiResponse.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateOrEditAsync: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        // DELETE: Xóa category
        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7053/api/Categories/{id}");

                var apiResponse = await response.Content
                    .ReadFromJsonAsync<ApiResponse<bool>>();

                if (apiResponse == null)
                {
                    return (false, "Failed to process response");
                }

                return (apiResponse.Success, apiResponse.Message ?? "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
