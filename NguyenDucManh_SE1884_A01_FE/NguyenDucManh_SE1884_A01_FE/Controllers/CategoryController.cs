using Assignmen_PRN232__.Dto;
using Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category/Index
        public async Task<IActionResult> Index(CategorySearchDto dto)
        {
            var searchDto = new CategorySearchDto
            {
                PageIndex = dto.PageIndex > 0 ? dto.PageIndex : 1,
                PageSize = dto.PageSize > 0 ? dto.PageSize : 10,
                Keyword = dto.Keyword
            };

            var result = await _categoryService.GetListPagingAsync(searchDto);

            // Truyền thêm search params để giữ lại khi phân trang
            ViewBag.CurrentPage = searchDto.PageIndex;
            ViewBag.PageSize = searchDto.PageSize;
            ViewBag.Keyword = searchDto.Keyword;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalRecords = result.TotalRecords;

            return View(result);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View("CreateEdit", new CategorySaveDto());
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }

            var saveDto = new CategorySaveDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };

            return View("CreateEdit", saveDto);
        }

        // POST: Category/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CategorySaveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                TempData["ErrorMessage"] = "Category name is required";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(dto.CategoryDesciption))
            {
                TempData["ErrorMessage"] = "Description is required";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input";
                return RedirectToAction(nameof(Index));
            }

            var result = await _categoryService.CreateOrEditAsync(dto);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // AJAX: Get form for create/edit modal
        [HttpGet]
        public async Task<IActionResult> GetCreateEditForm(int? id)
        {
            if (id.HasValue && id > 0)
            {
                // Edit mode - load data từ API
                var category = await _categoryService.GetByIdAsync(id.Value);
                if (category == null)
                    return BadRequest("Category not found");

                var saveDto = new CategorySaveDto
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    CategoryDesciption = category.CategoryDesciption,
                    ParentCategoryId = category.ParentCategoryId,
                    IsActive = category.IsActive
                };

                return PartialView("_CreateEditForm", saveDto);
            }
            else
            {
                // Create mode - form trống
                return PartialView("_CreateEditForm", new CategorySaveDto());
            }
        }
    }
}
