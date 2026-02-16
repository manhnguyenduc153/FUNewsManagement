using NguyenDucManh_SE1884_A01_BE.Services.IServices;
using NguyenDucManh_SE1884_A01_BE.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NguyenDucManh_SE1884_A01_BE.Models;

namespace NguyenDucManh_SE1884_A01_BE.Controllers.Api
{
    [Authorize(Roles = "Staff,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoriesController(ICategoryService categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetListPaging([FromQuery] CategorySearchDto dto)
        {
            var result = await _categoryService.GetListPagingAsync(dto);
            return Ok(result);
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        
        [HttpGet("odata")]
        [EnableQuery]
        public IActionResult GetOData()
        {
            return Ok(_context.Categories.AsQueryable());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Category not found" });
            return Ok(result);
        }

        
        [HttpPost("create-or-edit")]
        public async Task<IActionResult> CreateOrEdit([FromBody] CategorySaveDto dto)
        {
            var response = await _categoryService.CreateOrEditAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var response = await _categoryService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
