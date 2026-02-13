namespace NguyenDucManh_SE1884_A01_Analytic.Dto
{
    public class TrendingArticleDto
    {
        public string NewsArticleId { get; set; } = null!;
        public string NewsTitle { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public int ViewCount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class AnalyticsFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int Top { get; set; } = 10;
    }
}


    public class NewsArticleDto
    {
        public string NewsArticleId { get; set; } = null!;
        public string? NewsTitle { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedByName { get; set; }
        public int ViewCount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
