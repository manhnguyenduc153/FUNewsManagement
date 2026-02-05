namespace NguyenDucManh_SE1884_A01_Analytics.Dto
{
    public class DashboardDto
    {
        public List<CategoryStatDto> ArticlesByCategory { get; set; } = new();
        public StatusStatDto ArticlesByStatus { get; set; } = new();
    }

    public class CategoryStatDto
    {
        public string CategoryName { get; set; } = null!;
        public int Count { get; set; }
    }

    public class StatusStatDto
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
    }

    public class NewsArticleDto
    {
        public string? NewsArticleId { get; set; }
        public string? NewsTitle { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool? NewsStatus { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
