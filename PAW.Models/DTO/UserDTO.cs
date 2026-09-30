using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("passwordHash")]
    public string? PasswordHash { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static UserDTO ConvertFrom(User user)
    {
        return new UserDTO
        {
            Id = Guid.NewGuid(),
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            LastModified = user.LastModified,
            ModifiedBy = user.ModifiedBy,
            RoleId = user.RoleId,
            Comments = string.Empty,
            CreatedDate = user.CreatedAt ?? DateTime.Now,
            ModifiedDate = user.LastModified ?? DateTime.Now
        };
    }

    public static User ConvertTo(UserDTO userDTO)
    {
        return new User
        {
            UserId = userDTO.UserId,
            Username = userDTO.Username,
            Email = userDTO.Email,
            PasswordHash = userDTO.PasswordHash,
            CreatedAt = userDTO.CreatedAt,
            IsActive = userDTO.IsActive,
            LastModified = userDTO.LastModified,
            ModifiedBy = userDTO.ModifiedBy,
            RoleId = userDTO.RoleId
        };
    }
}
