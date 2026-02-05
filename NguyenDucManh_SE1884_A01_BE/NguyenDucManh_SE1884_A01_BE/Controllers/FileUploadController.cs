using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NguyenDucManh_SE1884_A01_BE.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long _maxFileSize = 5 * 1024 * 1024; // 5MB

    public FileUploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        if (file.Length > _maxFileSize)
            return BadRequest(new { message = "File size exceeds 5MB limit" });

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            return BadRequest(new { message = "Invalid file format. Only jpg, jpeg, png, gif, webp are allowed" });

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var imageUrl = $"/uploads/images/{uniqueFileName}";
        return Ok(new { imageUrl });
    }

    [HttpDelete("image")]
    public IActionResult DeleteImage([FromQuery] string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return BadRequest(new { message = "Image URL is required" });

        var fileName = Path.GetFileName(imageUrl);
        var filePath = Path.Combine(_environment.WebRootPath, "uploads", "images", fileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
            return Ok(new { message = "Image deleted successfully" });
        }

        return NotFound(new { message = "Image not found" });
    }
}
