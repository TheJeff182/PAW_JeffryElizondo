using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
using Task = PAW.Models.Task;

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
            if (task == null)
                return NotFound($"Task with id {id} not found");

            return Ok(PawTaskDTO.ConvertFrom(task));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<bool> Save([FromBody] PawTaskDTO pawTaskDTO)
        {
            try
            {
                var task = PawTaskDTO.ConvertTo(pawTaskDTO);

                if (pawTaskDTO.TaskId > 0)
                {
                    // Update
                    await taskRepository.UpdateAsync(task);
                }
                else
                {
                    // Create
                    await taskRepository.CreateAsync(task);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving task");
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async System.Threading.Tasks.Task<bool> Delete(int id)
        {
            try
            {
                var task = await taskRepository.FindAsync(id);
                if (task == null)
                    return false;

                await taskRepository.DeleteAsync(task);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting task with id {id}", id);
                return false;
            }
        }
    }
}
