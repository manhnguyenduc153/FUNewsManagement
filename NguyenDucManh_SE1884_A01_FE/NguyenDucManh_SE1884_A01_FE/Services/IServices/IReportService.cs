using Assignmen_PRN232__.Dto;

namespace Frontend.Services.IServices
{
    public interface IReportService
    {
        Task<NewsArticleReportDto?> GetNewsArticleReportAsync(ReportSearchDto searchDto);
    }
}
