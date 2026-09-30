using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("userActionId")]
    public decimal? UserActionId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static UserActionDTO ConvertFrom(UserAction userAction)
    {
        return new UserActionDTO
        {
            Id = Guid.NewGuid(),
            UserActionId = userAction.Id,
            Name = userAction.Name,
            Description = userAction.Description,
            Comments = string.Empty,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static UserAction ConvertTo(UserActionDTO userActionDTO)
    {
        return new UserAction
        {
            Id = userActionDTO.UserActionId,
            Name = userActionDTO.Name,
            Description = userActionDTO.Description
        };
    }
}
