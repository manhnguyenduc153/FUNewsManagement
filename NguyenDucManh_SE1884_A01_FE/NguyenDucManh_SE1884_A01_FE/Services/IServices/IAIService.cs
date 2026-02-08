namespace Frontend.Services.IServices
{
    public interface IAIService
    {
        Task<AISuggestTagsResponse> SuggestTagsAsync(string content);
    }

    public class AISuggestTagsResponse
    {
        public List<AITagSuggestion> SuggestedTags { get; set; } = new();
    }

    public class AITagSuggestion
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
    }
}
