using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserRoleDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("userRoleId")]
    public decimal? UserRoleId { get; set; }
    [JsonPropertyName("roleId")]
    public decimal? RoleId { get; set; }
    [JsonPropertyName("userId")]
    public decimal? UserId { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole userRole)
    {
        return new UserRoleDTO
        {
            Id = Guid.NewGuid(),
            UserRoleId = userRole.Id,
            RoleId = userRole.RoldId,
            UserId = userRole.UserId
        };
    }

    public static UserRole ConvertTo(UserRoleDTO userRoleDTO)
    {
        return new UserRole
        {
            Id = userRoleDTO.UserRoleId,
            RoldId = userRoleDTO.RoleId,
            UserId = userRoleDTO.UserId
        };
    }
}
