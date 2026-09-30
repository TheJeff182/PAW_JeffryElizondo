using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryId = category.CategoryId,
            Name = category.CategoryName!,
            Description = category.Description!,
            ModifiedBy = category.ModifiedBy,
            CreatedBy = string.Empty,
            Comments = string.Empty,
            CreatedDate = category.LastModified ?? DateTime.Now,
            ModifiedDate = category.LastModified ?? DateTime.Now
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
