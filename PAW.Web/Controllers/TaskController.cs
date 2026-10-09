using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _taskService.GetTasksAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
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
                pawTaskDTO.TaskId = 0;
                var result = await _taskService.CreateTaskAsync(pawTaskDTO);
                if (result)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error creating task");
            }

            return View(pawTaskDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return PartialView("_DeletePartial", task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (result)
                return RedirectToAction(nameof(Index));

            return BadRequest("Error deleting task");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
