using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class RoleDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    [JsonPropertyName("roleName")]
    public string? RoleName { get; set; }

    public static RoleDTO ConvertFrom(Role role)
    {
        return new RoleDTO
        {
            Id = Guid.NewGuid(),
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
    }

    public static Role ConvertTo(RoleDTO roleDTO)
    {
        return new Role
        {
            RoleId = roleDTO.RoleId,
            RoleName = roleDTO.RoleName
        };
    }
}
