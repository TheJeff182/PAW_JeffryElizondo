using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly IMinimalApiTaskService _minimalApiTaskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(IMinimalApiTaskService minimalApiTaskService, ILogger<TaskController> logger)
        {
            _minimalApiTaskService = minimalApiTaskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching tasks from Minimal API");
            var result = await _minimalApiTaskService.GetTasksAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _minimalApiTaskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return PartialView("_DetailsPartial", task);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PawTaskDTO pawTaskDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Creating new task: {TaskName}", pawTaskDTO.Name);
                    var result = await _minimalApiTaskService.CreateTaskAsync(pawTaskDTO);
                    if (result != null)
                        return RedirectToAction(nameof(Index));

                    ModelState.AddModelError("", "Error creating task");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating task");
                    ModelState.AddModelError("", "Error creating task: " + ex.Message);
                }
            }

            return View(pawTaskDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _minimalApiTaskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return PartialView("_DeletePartial", task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Deleting task with ID: {TaskId}", id);
                var result = await _minimalApiTaskService.DeleteTaskAsync(id);
                if (result)
                    return RedirectToAction(nameof(Index));

                return BadRequest("Error deleting task");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID: {TaskId}", id);
                return BadRequest("Error deleting task: " + ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
