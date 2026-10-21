using APW.Architecture;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

/// <summary>
/// Interface for consuming Tasks from Minimal API
/// </summary>
public interface IMinimalApiTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetTasksAsync();
    Task<PawTaskDTO?> GetTaskByIdAsync(int id);
    Task<PawTaskDTO?> CreateTaskAsync(PawTaskDTO taskDto);
    Task<bool> UpdateTaskAsync(int id, PawTaskDTO taskDto);
    Task<bool> DeleteTaskAsync(int id);
}

/// <summary>
/// Service for consuming Tasks from PAW.MinimalAPI
/// </summary>
public class MinimalApiTaskService : IMinimalApiTaskService
{
    private readonly IRestProvider _restProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MinimalApiTaskService> _logger;
    private string _baseUrl = string.Empty;

    public MinimalApiTaskService(
        IRestProvider restProvider,
        IConfiguration configuration,
        ILogger<MinimalApiTaskService> logger)
    {
        _restProvider = restProvider;
        _configuration = configuration;
        _logger = logger;

        // Get Minimal API URL from configuration
        _baseUrl = _configuration["ApiEndpoints:MinimalApi"] ?? "http://localhost:5167";
    }

    public async Task<IEnumerable<PawTaskDTO>> GetTasksAsync()
    {
        try
        {
            var endpoint = $"{_baseUrl}/api/tasks";
            var response = await _restProvider.GetAsync(endpoint, null);

            // Deserialize the array of TaskItem from Minimal API
            var tasks = await JsonSerializer.DeserializeAsync<List<TaskItemDto>>(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response)),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Map TaskItem to PawTaskDTO
            var result = tasks?.Select(MapToPawTaskDTO).ToList() ?? new List<PawTaskDTO>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tasks from Minimal API");
            return Enumerable.Empty<PawTaskDTO>();
        }
    }

    public async Task<PawTaskDTO?> GetTaskByIdAsync(int id)
    {
        try
        {
            var endpoint = $"{_baseUrl}/api/tasks/{id}";
            var response = await _restProvider.GetAsync(endpoint, id.ToString());

            var task = await JsonSerializer.DeserializeAsync<TaskItemDto>(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response)),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return task != null ? MapToPawTaskDTO(task) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching task {Id} from Minimal API", id);
            return null;
        }
    }

    public async Task<PawTaskDTO?> CreateTaskAsync(PawTaskDTO taskDto)
    {
        try
        {
            var endpoint = $"{_baseUrl}/api/tasks";
            var taskItem = new
            {
                taskDto.Name,
                taskDto.Description,
                taskDto.Status,
                taskDto.DueDate,
                taskDto.ModifiedBy
            };
            var json = JsonSerializer.Serialize(taskItem);
            var response = await _restProvider.PostAsync(endpoint, json);

            var createdTask = await JsonSerializer.DeserializeAsync<TaskItemDto>(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response)),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return createdTask != null ? MapToPawTaskDTO(createdTask) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task in Minimal API");
            return null;
        }
    }

    public async Task<bool> UpdateTaskAsync(int id, PawTaskDTO taskDto)
    {
        try
        {
            var endpoint = $"{_baseUrl}/api/tasks/{id}";
            var taskItem = new
            {
                taskDto.Name,
                taskDto.Description,
                taskDto.Status,
                taskDto.DueDate,
                taskDto.ModifiedBy
            };
            var json = JsonSerializer.Serialize(taskItem);
            var response = await _restProvider.PutAsync(endpoint, id.ToString(), json);

            return !string.IsNullOrEmpty(response) && 
                   !response.Contains("error", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {Id} in Minimal API", id);
            return false;
        }
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        try
        {
            var endpoint = $"{_baseUrl}/api/tasks/{id}";
            var response = await _restProvider.DeleteAsync(endpoint, id.ToString());

            return !string.IsNullOrEmpty(response) && 
                   !response.Contains("error", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {Id} from Minimal API", id);
            return false;
        }
    }

    /// <summary>
    /// Helper class to deserialize TaskItem from Minimal API response
    /// </summary>
    private class TaskItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? ModifiedBy { get; set; }
    }

    /// <summary>
    /// Maps TaskItemDto from Minimal API to PawTaskDTO for MVC
    /// </summary>
    private static PawTaskDTO MapToPawTaskDTO(TaskItemDto taskItem)
    {
        return new PawTaskDTO
        {
            Id = Guid.NewGuid(),
            TaskId = taskItem.Id,
            Name = taskItem.Name,
            Description = taskItem.Description,
            Status = taskItem.Status,
            DueDate = taskItem.DueDate,
            CreatedAt = taskItem.CreatedAt,
            LastModified = taskItem.LastModified,
            ModifiedBy = taskItem.ModifiedBy
        };
    }
}
