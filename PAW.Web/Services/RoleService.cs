using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<bool> CreateRoleAsync(RoleDTO roleDTO);
    Task<bool> UpdateRoleAsync(RoleDTO roleDTO);
    Task<bool> DeleteRoleAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var roles = await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
        return roles;
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var role = await JsonProvider.DeserializeAsync<RoleDTO>(response);
        return role;
    }

    public async Task<bool> CreateRoleAsync(RoleDTO roleDTO)
    {
        roleDTO.RoleId = 0;
        var json = JsonSerializer.Serialize(roleDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateRoleAsync(RoleDTO roleDTO)
    {
        var json = JsonSerializer.Serialize(roleDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
