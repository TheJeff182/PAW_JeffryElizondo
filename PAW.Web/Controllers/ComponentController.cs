using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService componentService, ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _componentService.GetComponentsAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
                return NotFound();

            return PartialView("_DetailsPartial", component);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentDTO? component)
        {
            _logger.LogInformation($"Create POST called with component: {component?.Name}");

            if (component == null)
            {
                _logger.LogWarning("Create POST: component is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"ModelState invalid. Errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return View(component);
            }

            component.ComponentId = 0;
            var result = await _componentService.CreateComponentAsync(component);
            _logger.LogInformation($"CreateComponentAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("CreateComponentAsync returned false");
            return View(component);
        }

        public async Task<IActionResult> Delete(decimal id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
                return NotFound();

            return PartialView("_DeletePartial", component);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            _logger.LogInformation($"DeleteConfirmed POST called with id: {id}");

            var result = await _componentService.DeleteComponentAsync(id);
            _logger.LogInformation($"DeleteComponentAsync result: {result}");

            if (result)
                return RedirectToAction(nameof(Index));

            _logger.LogWarning("DeleteComponentAsync returned false");
            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
