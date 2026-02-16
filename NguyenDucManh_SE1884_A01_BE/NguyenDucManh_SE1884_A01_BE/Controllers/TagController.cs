using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Dto.Common;
using NguyenDucManh_SE1884_A01_BE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NguyenDucManh_SE1884_A01_BE.Models;

namespace NguyenDucManh_SE1884_A01_BE.Controllers.Api
{
    [Authorize(Roles = "Staff,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly AppDbContext _context;

        public TagsController(ITagService tagService, AppDbContext context)
        {
            _tagService = tagService;
            _context = context;
        }

        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAllAsync();
            return Ok(result);
        }

        
        [AllowAnonymous]
        [HttpGet("odata")]
        [EnableQuery]
        public IActionResult GetOData()
        {
            return Ok(_context.Tags.AsQueryable());
        }

        
        [HttpGet]
        public async Task<IActionResult> GetListPaging([FromQuery] TagSearchDto dto)
        {
            var result = await _tagService.GetListPagingAsync(dto);
            return Ok(ApiResponse<object>.Ok(result, "Get list successfully"));
        }

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("Tag not found", StatusCodes.Status404NotFound));
            return Ok(ApiResponse<TagDto>.Ok(result, "Get tag successfully"));
        }

        
        [HttpPost("create-or-edit")]
        public async Task<IActionResult> CreateOrEdit([FromBody] TagSaveDto dto)
        {
            var response = await _tagService.CreateOrEditAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _tagService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        
        [AllowAnonymous]
        [HttpGet("{id:int}/articles")]
        public async Task<IActionResult> GetArticlesByTag(int id)
        {
            var result = await _tagService.GetArticlesByTagAsync(id);
            return Ok(ApiResponse<object>.Ok(result, "Get articles successfully"));
        }
    }
}
