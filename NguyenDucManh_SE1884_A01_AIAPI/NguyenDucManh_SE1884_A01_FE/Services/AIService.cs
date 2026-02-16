using NguyenDucManh_SE1884_A01_AIAPI.Dto;
using NguyenDucManh_SE1884_A01_AIAPI.Services.IServices;
using System.Text;
using System.Text.Json;

namespace NguyenDucManh_SE1884_A01_AIAPI.Services
{
    public class AIService : IAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AIService> _logger;
        private readonly ITagLearningCache _tagLearningCache;
        private const string GEMINI_API_KEY = "AIzaSyBvyiqJ8Uumh3IxsW-m1IcjUQoWeG9nc80";
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";


        public AIService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AIService> logger, ITagLearningCache tagLearningCache)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _tagLearningCache = tagLearningCache;
        }

        public async Task<SuggestTagsResponse> SuggestTagsAsync(string content)
        {
            _logger.LogInformation("[AI] Starting tag suggestion for content length: {Length}", content.Length);
            
            // 1. Get all tags from BE API
            var allTags = await GetAllTagsFromBEAsync();
            _logger.LogInformation("[AI] Retrieved {Count} tags from BE", allTags.Count);
            
            if (!allTags.Any())
            {
                _logger.LogWarning("[AI] No tags found from BE API");
                return new SuggestTagsResponse();
            }

            // 2. Call Gemini AI to analyze content
            var suggestedTagNames = await AnalyzeContentWithGeminiAsync(content, allTags);
            _logger.LogInformation("[AI] Gemini suggested {Count} tags: {Tags}", suggestedTagNames.Count, string.Join(", ", suggestedTagNames));
            
            // 2.5. Boost frequently selected tags
            var frequentTagIds = _tagLearningCache.GetFrequentTags(3);
            var frequentTags = allTags.Where(t => frequentTagIds.Contains(t.TagId)).Select(t => t.TagName).ToList();
            _logger.LogInformation("[AI] Frequent tags from cache: {Tags}", string.Join(", ", frequentTags));
            
            if (!suggestedTagNames.Any())
            {
                _logger.LogWarning("[AI] Gemini returned no suggestions");
                return new SuggestTagsResponse();
            }

            // 3. Match suggested names with actual tags (flexible matching)
            var matchedTags = new List<TagSuggestion>();
            
            // Prioritize frequent tags if they match
            foreach (var frequentTagId in frequentTagIds)
            {
                var frequentTag = allTags.FirstOrDefault(t => t.TagId == frequentTagId);
                if (frequentTag != null && suggestedTagNames.Any(s => 
                    s.Equals(frequentTag.TagName, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(frequentTag.TagName, StringComparison.OrdinalIgnoreCase)))
                {
                    matchedTags.Add(new TagSuggestion { TagId = frequentTag.TagId, TagName = frequentTag.TagName });
                    _logger.LogInformation("[AI] Boosted frequent tag: {Tag}", frequentTag.TagName);
                }
            }
            
            foreach (var suggestedName in suggestedTagNames)
            {
                var matchedTag = allTags.FirstOrDefault(t => 
                    t.TagName.Equals(suggestedName, StringComparison.OrdinalIgnoreCase) ||
                    t.TagName.Contains(suggestedName, StringComparison.OrdinalIgnoreCase) ||
                    suggestedName.Contains(t.TagName, StringComparison.OrdinalIgnoreCase)
                );
                
                if (matchedTag != null && !matchedTags.Any(mt => mt.TagId == matchedTag.TagId))
                {
                    _logger.LogInformation("[AI] Matched '{Suggested}' -> '{Actual}' (ID: {Id})", suggestedName, matchedTag.TagName, matchedTag.TagId);
                    matchedTags.Add(new TagSuggestion { TagId = matchedTag.TagId, TagName = matchedTag.TagName });
                }
                else
                {
                    _logger.LogWarning("[AI] Could not match suggested tag: {Tag}", suggestedName);
                }
            }

            _logger.LogInformation("[AI] Final matched tags: {Count}", matchedTags.Count);
            return new SuggestTagsResponse { SuggestedTags = matchedTags };
        }

        private async Task<List<TagDto>> GetAllTagsFromBEAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var beApiUrl = _configuration["CoreApiUrl"] ?? "https://localhost:7053";
            
            try
            {
                _logger.LogInformation("[AI] Calling BE API: {Url}/api/tags/all", beApiUrl);
                var response = await client.GetAsync($"{beApiUrl}/api/tags/all");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[AI] BE API response: {Json}", json);
                    
                    return JsonSerializer.Deserialize<List<TagDto>>(json, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    }) ?? new List<TagDto>();
                }
                else
                {
                    _logger.LogError("[AI] BE API failed: {Status}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AI] Error calling BE API");
            }
            
            return new List<TagDto>();
        }

        private async Task<List<string>> AnalyzeContentWithGeminiAsync(string content, List<TagDto> availableTags)
        {
            var client = _httpClientFactory.CreateClient();
            var tagList = string.Join(", ", availableTags.Select(t => t.TagName));
            
            _logger.LogInformation("[AI] Available tags for Gemini: {Tags}", tagList);
            
            var prompt = $@"You are a news article tag classifier. Analyze the content and select the most relevant tags.

Available tags: {tagList}

Article content: {content}

Instructions:
- Select 3-5 most relevant tags from the available tags list
- Return ONLY the tag names, separated by commas
- Use exact tag names from the list
- No explanations or additional text

Tags:";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.3,
                    maxOutputTokens = 100
                }
            };

            try
            {
                var json = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogInformation("[AI] Calling Gemini API...");
                var response = await client.PostAsync($"{GEMINI_API_URL}?key={GEMINI_API_KEY}", httpContent);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[AI] Gemini response: {Response}", responseJson);
                    
                    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    var aiText = geminiResponse?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "";
                    _logger.LogInformation("[AI] Gemini AI text: {Text}", aiText);
                    
                    // Clean and parse AI response
                    var tags = aiText
                        .Replace("Tags:", "")
                        .Replace("tags:", "")
                        .Split(new[] { ',', '\n', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim().Trim('"').Trim('\'').Trim())
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Take(5)
                        .ToList();
                    
                    return tags;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("[AI] Gemini API failed: {Status}, {Error}", response.StatusCode, errorContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AI] Error calling Gemini API");
            }

            return new List<string>();
        }

        public void RecordTagSelections(List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                _tagLearningCache.RecordTagSelection(tagId);
            }
            _logger.LogInformation("[AI] Recorded {Count} tag selections", tagIds.Count);
        }

        private class TagDto
        {
            public int TagId { get; set; }
            public string TagName { get; set; } = string.Empty;
        }

        private class GeminiResponse
        {
            public List<Candidate>? candidates { get; set; }
        }

        private class Candidate
        {
            public Content? content { get; set; }
        }

        private class Content
        {
            public List<Part>? parts { get; set; }
        }

        private class Part
        {
            public string? text { get; set; }
        }
    }
}
