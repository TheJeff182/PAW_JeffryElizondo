using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("name")]
    [Required(ErrorMessage = "The Name field is required.")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime? ModifiedDate { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryId = category.CategoryId,
            Name = category.CategoryName ?? string.Empty,
            Description = category.Description,
            ModifiedBy = category.ModifiedBy,
            ModifiedDate = category.LastModified
        };
    }

    public static Category ConvertTo(CategoryDTO categoryDTO)
    {
        return new Category
        {
            CategoryId = categoryDTO.CategoryId,
            CategoryName = categoryDTO.Name,
            Description = categoryDTO.Description,
            ModifiedBy = categoryDTO.ModifiedBy,
            LastModified = categoryDTO.ModifiedDate
        };
    }
}

