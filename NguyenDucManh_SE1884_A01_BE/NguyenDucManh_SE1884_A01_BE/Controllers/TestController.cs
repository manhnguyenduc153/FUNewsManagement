using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NguyenDucManh_SE1884_A01_BE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new { message = "Public endpoint works!" });
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isAuthenticated = User.Identity?.IsAuthenticated;

        return Ok(new
        {
            message = "Protected endpoint works!",
            isAuthenticated,
            userId,
            email,
            name,
            role,
            allClaims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
