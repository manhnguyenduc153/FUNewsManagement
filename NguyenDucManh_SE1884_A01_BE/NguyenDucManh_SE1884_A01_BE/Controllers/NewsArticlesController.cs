using NguyenDucManh_SE1884_A01_BE.Services.IServices;
using NguyenDucManh_SE1884_A01_BE.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NguyenDucManh_SE1884_A01_BE.Controllers.Api
{
    [Authorize(Roles = "Admin,Staff")]
    [ApiController]
    [Route("api/[controller]")]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetListPaging([FromQuery] NewsArticleSearchDto dto)
        {
            var result = await _newsArticleService.GetListPagingAsync(dto);
            return Ok(result);
        }

        
        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<IActionResult> GetPublicListPaging([FromQuery] NewsArticleSearchDto dto)
        {
            var result = await _newsArticleService.GetPublicListPagingAsync(dto);
            return Ok(result);
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _newsArticleService.GetAllAsync();
            return Ok(result);
        }

        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _newsArticleService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "News article not found" });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("{id}/increment-view")]
        public async Task<IActionResult> IncrementView(string id)
        {
            var response = await _newsArticleService.IncrementViewCountAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpPost("create-or-edit")]
        public async Task<IActionResult> CreateOrEdit([FromBody] NewsArticleSaveDto dto)
        {
            var response = await _newsArticleService.CreateOrEditAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpPost("{id}/duplicate")]
        public async Task<IActionResult> Duplicate(string id)
        {
            var response = await _newsArticleService.DuplicateAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _newsArticleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpPost("{id}/tags/{tagId:int}")]
        public async Task<IActionResult> AddTag(string id, int tagId)
        {
            var response = await _newsArticleService.AddTagAsync(id, tagId);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id}/tags/{tagId:int}")]
        public async Task<IActionResult> RemoveTag(string id, int tagId)
        {
            var response = await _newsArticleService.RemoveTagAsync(id, tagId);
            return StatusCode(response.StatusCode, response);
        }

        
        [AllowAnonymous]
        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelatedArticles(string id)
        {
            var result = await _newsArticleService.GetRelatedArticlesAsync(id);
            return Ok(result);
        }
    }
}
