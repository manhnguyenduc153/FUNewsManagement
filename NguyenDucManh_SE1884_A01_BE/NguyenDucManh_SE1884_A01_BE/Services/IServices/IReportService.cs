using NguyenDucManh_SE1884_A01_BE.Dto;

namespace NguyenDucManh_SE1884_A01_BE.Services.IServices
{
    public interface IReportService
    {
        Task<NewsArticleReportDto> GetNewsArticleReportAsync(ReportSearchDto searchDto);
    }
}
