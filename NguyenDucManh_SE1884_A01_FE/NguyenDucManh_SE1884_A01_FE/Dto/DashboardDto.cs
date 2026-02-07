using System.Text.Json.Serialization;

namespace Assignmen_PRN232__.Dto
{
    public class DashboardDto
    {
        [JsonPropertyName("articlesByCategory")]
        public List<DashboardCategoryStatDto> ArticlesByCategory { get; set; } = new();
        
        [JsonPropertyName("articlesByStatus")]
        public DashboardStatusStatDto ArticlesByStatus { get; set; } = new();
    }

    public class DashboardCategoryStatDto
    {
        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = null!;
        
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class DashboardStatusStatDto
    {
        [JsonPropertyName("activeCount")]
        public int ActiveCount { get; set; }
        
        [JsonPropertyName("inactiveCount")]
        public int InactiveCount { get; set; }
    }
}
