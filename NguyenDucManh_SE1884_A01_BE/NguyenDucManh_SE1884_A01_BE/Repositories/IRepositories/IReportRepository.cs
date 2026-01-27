using NguyenDucManh_SE1884_A01_BE.Dto;

namespace NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories
{
    public interface IReportRepository
    {
        Task<NewsArticleReportDto> GetNewsArticleReportAsync(ReportSearchDto searchDto);
    }
}
