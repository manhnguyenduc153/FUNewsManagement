using System.Text.Json.Serialization;

namespace NguyenDucManh_SE1884_A01_Analytic.Dto
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

    public class NewsArticleReportDto
    {
        [JsonPropertyName("categoryStats")]
        public List<ArticleByCategoryDto> CategoryStats { get; set; } = new();
        
        [JsonPropertyName("totalActive")]
        public int TotalActive { get; set; }
        
        [JsonPropertyName("totalInactive")]
        public int TotalInactive { get; set; }
    }

    public class ArticleByCategoryDto
    {
        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = null!;
        
        [JsonPropertyName("articleCount")]
        public int ArticleCount { get; set; }
    }
}
