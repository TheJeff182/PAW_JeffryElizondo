using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
using Task = PAW.Models.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(ILogger<TaskController> logger, ITaskRepository taskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetTasks")]
        public async System.Threading.Tasks.Task<IEnumerable<PawTaskDTO>> GetAll()
        {
            var tasks = await taskRepository.ReadAsync() ?? [];
            return tasks.Select(PawTaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async System.Threading.Tasks.Task<ActionResult<PawTaskDTO>> GetById(int id)
        {
            var task = await taskRepository.FindAsync(id);
            return PawTaskDTO.ConvertFrom(task);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<bool> Save([FromBody] IEnumerable<Task> Tasks)
        {
            foreach (var t in Tasks)
            {
                if (t.Id > 0)
                    await taskRepository.CreateAsync(t);
                else 
                    await taskRepository.UpdateAsync(t);
            }

            return true;
        }
    }
}
