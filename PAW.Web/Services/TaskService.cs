using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface ITaskService
{
    Task<IEnumerable<PawTaskDTO>> GetTasksAsync();
    Task<PawTaskDTO?> GetTaskByIdAsync(int id);
    Task<bool> CreateTaskAsync(PawTaskDTO pawTaskDTO);
    Task<bool> UpdateTaskAsync(PawTaskDTO pawTaskDTO);
    Task<bool> DeleteTaskAsync(int id);
}

public class TaskService : ServiceBase, ITaskService
{
    private const string _path = "Task";
    private readonly IRestProvider _restProvider;

    public TaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawTaskDTO>> GetTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var tasks = await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
        return tasks;
    }

    public async Task<PawTaskDTO?> GetTaskByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        var task = await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
        return task;
    }

    public async Task<bool> CreateTaskAsync(PawTaskDTO pawTaskDTO)
    {
        var json = JsonSerializer.Serialize(pawTaskDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateTaskAsync(PawTaskDTO pawTaskDTO)
    {
        var json = JsonSerializer.Serialize(pawTaskDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
