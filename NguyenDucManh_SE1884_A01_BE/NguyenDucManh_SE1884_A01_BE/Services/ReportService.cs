using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories;
using NguyenDucManh_SE1884_A01_BE.Services.IServices;

namespace NguyenDucManh_SE1884_A01_BE.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<NewsArticleReportDto> GetNewsArticleReportAsync(ReportSearchDto searchDto)
        {
            return await _reportRepository.GetNewsArticleReportAsync(searchDto);
        }
    }
}
