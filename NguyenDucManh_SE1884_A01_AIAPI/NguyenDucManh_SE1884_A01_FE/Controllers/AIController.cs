using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NguyenDucManh_SE1884_A01_AIAPI.Dto;
using NguyenDucManh_SE1884_A01_AIAPI.Services.IServices;

namespace NguyenDucManh_SE1884_A01_AIAPI.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("suggest-tags")]
        public async Task<IActionResult> SuggestTags([FromBody] SuggestTagsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest(new { message = "Content is required" });
            }

            if (request.Content.Length > 5000)
            {
                return BadRequest(new { message = "Content exceeds maximum length of 5000 characters" });
            }

            var result = await _aiService.SuggestTagsAsync(request.Content);
            return Ok(result);
        }

        [HttpPost("record-tag-selection")]
        public IActionResult RecordTagSelection([FromBody] RecordTagRequest request)
        {
            if (request.TagIds == null || !request.TagIds.Any())
            {
                return BadRequest(new { message = "TagIds are required" });
            }

            _aiService.RecordTagSelections(request.TagIds);
            return Ok(new { message = "Tag selections recorded" });
        }
    }
}
