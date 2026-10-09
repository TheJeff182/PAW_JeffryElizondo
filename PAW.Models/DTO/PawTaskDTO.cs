using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("taskId")]
    public int TaskId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static PawTaskDTO ConvertFrom(Task task)
    {
        return new PawTaskDTO
        {
            Id = Guid.NewGuid(),
            TaskId = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified,
            ModifiedBy = task.ModifiedBy
        };
    }

    public static Task ConvertTo(PawTaskDTO pawTaskDTO)
    {
        return new Task
        {
            Id = pawTaskDTO.TaskId,
            Name = pawTaskDTO.Name,
            Description = pawTaskDTO.Description,
            Status = pawTaskDTO.Status,
            DueDate = pawTaskDTO.DueDate,
            CreatedAt = pawTaskDTO.CreatedAt,
            LastModified = pawTaskDTO.LastModified,
            ModifiedBy = pawTaskDTO.ModifiedBy
        };
    }
}
