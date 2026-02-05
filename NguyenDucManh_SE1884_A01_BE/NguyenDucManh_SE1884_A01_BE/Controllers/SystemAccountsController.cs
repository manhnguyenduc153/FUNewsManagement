using NguyenDucManh_SE1884_A01_BE.Dto;
using NguyenDucManh_SE1884_A01_BE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NguyenDucManh_SE1884_A01_BE.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetListPaging([FromQuery] SystemAccountSearchDto dto)
        {
            var result = await _systemAccountService.GetListPagingAsync(dto);
            return Ok(result);
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _systemAccountService.GetAllAsync();
            return Ok(result);
        }

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(short id)
        {
            var result = await _systemAccountService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Account not found" });
            return Ok(result);
        }

        
        [HttpPost("create-or-edit")]
        public async Task<IActionResult> CreateOrEdit([FromBody] SystemAccountSaveDto dto)
        {
            var response = await _systemAccountService.CreateOrEditAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(short id)
        {
            var response = await _systemAccountService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
