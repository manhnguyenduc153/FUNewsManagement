namespace NguyenDucManh_SE1884_A01_AIAPI.Dto
{
    public class SuggestTagsRequest
    {
        public string Content { get; set; } = string.Empty;
    }

    public class SuggestTagsResponse
    {
        public List<TagSuggestion> SuggestedTags { get; set; } = new();
    }

    public class TagSuggestion
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
    }
}
