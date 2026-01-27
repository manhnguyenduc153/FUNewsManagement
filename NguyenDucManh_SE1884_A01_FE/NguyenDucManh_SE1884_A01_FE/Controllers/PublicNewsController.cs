using Assignmen_PRN232__.Dto;
using Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class PublicNewsController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public PublicNewsController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public async Task<IActionResult> Index(NewsArticleSearchDto searchDto)
        {
            searchDto.PageIndex = searchDto.PageIndex <= 0 ? 1 : searchDto.PageIndex;
            searchDto.PageSize = searchDto.PageSize <= 0 ? 10 : searchDto.PageSize;

            var result = await _newsArticleService.GetPublicListPagingAsync(searchDto);
            return View(result);
        }

        public async Task<IActionResult> Details(string id)
        {
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found";
                return RedirectToAction(nameof(Index));
            }

            var relatedArticles = await _newsArticleService.GetRelatedArticlesAsync(id);
            ViewBag.RelatedArticles = relatedArticles;

            return View(article);
        }
    }
}
