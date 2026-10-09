using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private const int PageSize = 25;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allProducts = (await _productService.GetProductsAsync())?.ToList() ?? [];

            var totalItems = allProducts.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedProducts = allProducts
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = PageSize;

            return View(paginatedProducts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return PartialView("_DetailsPartial", product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            _logger.LogInformation($"Create POST called with product: {product?.Name}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"ModelState invalid. Errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return View(product);
            }

            product.ProductId = 0;
            var result = await _productService.CreateProductAsync(product);
            _logger.LogInformation($"CreateProductAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("CreateProductAsync returned false");
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDTO product)
        {
            _logger.LogInformation($"Edit POST called with id: {id}, product: {product?.Name}");

            if (id != product.ProductId)
            {
                _logger.LogWarning($"ID mismatch: {id} != {product?.ProductId}");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"ModelState invalid. Errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return View(product);
            }

            var result = await _productService.UpdateProductAsync(product);
            _logger.LogInformation($"UpdateProductAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("UpdateProductAsync returned false");
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return PartialView("_DeletePartial", product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result)
                return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Delete), new { id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
