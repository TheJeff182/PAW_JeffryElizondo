using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("componentId")]
    public decimal ComponentId { get; set; }

    [JsonPropertyName("name")]
    [Required(ErrorMessage = "The Name field is required.")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    [Required(ErrorMessage = "The Content field is required.")]
    public string Content { get; set; }

    public static ComponentDTO ConvertFrom(Component component)
    {
        return new ComponentDTO
        {
            Id = Guid.NewGuid(),
            ComponentId = component.Id,
            Name = component.Name,
            Content = component.Content
        };
    }

    public static Component ConvertTo(ComponentDTO componentDTO)
    {
        return new Component
        {
            Id = componentDTO.ComponentId,
            Name = componentDTO.Name,
            Content = componentDTO.Content
        };
    }
}
