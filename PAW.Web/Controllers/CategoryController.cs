using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return PartialView("_DetailsPartial", category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO? category)
        {
            _logger.LogInformation($"Create POST called with category: {category?.Name}");

            if (category == null)
            {
                _logger.LogWarning("Create POST: category is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"ModelState invalid. Errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return View(category);
            }

            category.CategoryId = 0;
            var result = await _categoryService.CreateCategoryAsync(category);
            _logger.LogInformation($"CreateCategoryAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("CreateCategoryAsync returned false");
            return View(category);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return PartialView("_DeletePartial", category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"DeleteConfirmed POST called with id: {id}");

            var result = await _categoryService.DeleteCategoryAsync(id);
            _logger.LogInformation($"DeleteCategoryAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("DeleteCategoryAsync returned false");
            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
