using Assignmen_PRN232__.Dto;
using Frontend.Services;
using Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IJwtHelperService _jwtHelper;
        private readonly IAIService _aiService;
        private readonly ILogger<NewsArticleController> _logger;

        public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IJwtHelperService jwtHelper, IAIService aiService, ILogger<NewsArticleController> logger)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _jwtHelper = jwtHelper;
            _aiService = aiService;
            _logger = logger;
        }

        // GET: NewsArticle/Index
        public async Task<IActionResult> Index(NewsArticleSearchDto dto)
        {
            var searchDto = new NewsArticleSearchDto
            {
                PageIndex = dto.PageIndex > 0 ? dto.PageIndex : 1,
                PageSize = dto.PageSize > 0 ? dto.PageSize : 10,
                Keyword = dto.Keyword,
                Title = dto.Title,
                Author = dto.Author,
                CategoryId = dto.CategoryId,
                Status = dto.Status,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate
            };

            var result = await _newsArticleService.GetListPagingAsync(searchDto);

            ViewBag.Categories = await _categoryService.GetAllAsync();

            ViewBag.CurrentPage = searchDto.PageIndex;
            ViewBag.PageSize = searchDto.PageSize;
            ViewBag.Keyword = searchDto.Keyword;
            ViewBag.Title = searchDto.Title;
            ViewBag.Author = searchDto.Author;
            ViewBag.CategoryId = searchDto.CategoryId;
            ViewBag.Status = searchDto.Status;
            ViewBag.FromDate = searchDto.FromDate;
            ViewBag.ToDate = searchDto.ToDate;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalRecords = result.TotalRecords;

            return View(result);
        }

        // GET: NewsArticle/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View("CreateEdit", new NewsArticleSaveDto());
        }

        // GET: NewsArticle/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var newsArticle = await _newsArticleService.GetByIdAsync(id);
            if (newsArticle == null)
            {
                TempData["ErrorMessage"] = "News Article not found";
                return RedirectToAction(nameof(Index));
            }

            var saveDto = new NewsArticleSaveDto
            {
                NewsArticleId = newsArticle.NewsArticleId,
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                CreatedDate = newsArticle.CreatedDate,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                CategoryId = newsArticle.CategoryId,
                NewsStatus = newsArticle.NewsStatus,
                CreatedById = newsArticle.CreatedById,
                UpdatedById = newsArticle.UpdatedById,
                ModifiedDate = newsArticle.ModifiedDate
            };

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View("CreateEdit", saveDto);
        }

        // POST: NewsArticle/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(NewsArticleSaveDto dto)
        {
            var userInfo = _jwtHelper.GetUserInfo();
            var redirectAction = userInfo.Role == "Staff" ? nameof(MyArticles) : nameof(Index);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorMessage = string.Join("; ", errors.Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = "Invalid input: " + errorMessage;
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return RedirectToAction(redirectAction);
            }

            var result = await _newsArticleService.CreateOrEditAsync(dto);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(redirectAction);
        }

        // POST: NewsArticle/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var userInfo = _jwtHelper.GetUserInfo();
            var result = await _newsArticleService.DeleteAsync(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(userInfo.Role == "Staff" ? nameof(MyArticles) : nameof(Index));
        }

        // GET: NewsArticle/MyArticles
        public async Task<IActionResult> MyArticles(NewsArticleSearchDto dto)
        {
            var userInfo = _jwtHelper.GetUserInfo();
            var token = HttpContext.Session.GetString("AccessToken");
            
            if (!userInfo.IsAuthenticated || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Index", "Login");
            }

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out var userId))
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Index", "Login");
            }

            var searchDto = new NewsArticleSearchDto
            {
                PageIndex = dto.PageIndex > 0 ? dto.PageIndex : 1,
                PageSize = dto.PageSize > 0 ? dto.PageSize : 10,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                Status = dto.Status,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                CreatedById = userId
            };

            var result = await _newsArticleService.GetListPagingAsync(searchDto);

            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.CurrentPage = searchDto.PageIndex;
            ViewBag.PageSize = searchDto.PageSize;
            ViewBag.Title = searchDto.Title;
            ViewBag.CategoryId = searchDto.CategoryId;
            ViewBag.Status = searchDto.Status;
            ViewBag.FromDate = searchDto.FromDate;
            ViewBag.ToDate = searchDto.ToDate;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalRecords = result.TotalRecords;

            return View(result);
        }

        // AJAX: Get form for create/edit modal
        [HttpGet]
        public async Task<IActionResult> GetCreateEditForm(string? id)
        {
            var categories = await _categoryService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Tags = tags;

            if (!string.IsNullOrEmpty(id))
            {
                // Edit mode - load data từ API
                var newsArticle = await _newsArticleService.GetByIdAsync(id);
                if (newsArticle == null)
                    return BadRequest("News Article not found");

                var saveDto = new NewsArticleSaveDto
                {
                    NewsArticleId = newsArticle.NewsArticleId,
                    NewsTitle = newsArticle.NewsTitle,
                    Headline = newsArticle.Headline,
                    CreatedDate = newsArticle.CreatedDate,
                    NewsContent = newsArticle.NewsContent,
                    NewsSource = newsArticle.NewsSource,
                    CategoryId = newsArticle.CategoryId,
                    NewsStatus = newsArticle.NewsStatus,
                    CreatedById = newsArticle.CreatedById,
                    UpdatedById = newsArticle.UpdatedById,
                    ModifiedDate = newsArticle.ModifiedDate,
                    ImageUrl = newsArticle.ImageUrl,
                    TagIds = newsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>()
                };

                return PartialView("_CreateEditForm", saveDto);
            }
            else
            {
                // Create mode - form trống
                return PartialView("_CreateEditForm", new NewsArticleSaveDto());
            }
        }

        // AJAX: Get all tags for dropdown
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllAsync();
            return Json(tags);
        }

        // AJAX: Add tag to news article
        [HttpPost]
        public async Task<IActionResult> AddTag(string id, int tagId)
        {
            var result = await _newsArticleService.AddTagAsync(id, tagId);
            return Json(new { success = result.Success, message = result.Message });
        }

        // AJAX: Remove tag from news article
        [HttpPost]
        public async Task<IActionResult> RemoveTag(string id, int tagId)
        {
            var result = await _newsArticleService.RemoveTagAsync(id, tagId);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> Duplicate(string id)
        {
            var userInfo = _jwtHelper.GetUserInfo();
            var result = await _newsArticleService.DuplicateAsync(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(userInfo.Role == "Staff" ? nameof(MyArticles) : nameof(Index));
        }

        // AJAX: AI Suggest Tags
        [HttpPost]
        public async Task<IActionResult> AISuggestTags([FromBody] AISuggestRequest request)
        {
            _logger.LogInformation("[FE] AISuggestTags called with content length: {Length}", request?.Content?.Length ?? 0);
            
            if (string.IsNullOrWhiteSpace(request?.Content))
            {
                _logger.LogWarning("[FE] Content is empty");
                return Json(new { success = false, message = "Content is required" });
            }

            try
            {
                _logger.LogInformation("[FE] Calling AI Service...");
                var result = await _aiService.SuggestTagsAsync(request.Content);
                _logger.LogInformation("[FE] AI Service returned {Count} tags", result.SuggestedTags.Count);
                return Json(new { success = true, suggestedTags = result.SuggestedTags });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FE] Error calling AI Service");
                return Json(new { success = false, message = "Error calling AI service" });
            }
        }
    }

    public class AISuggestRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}
