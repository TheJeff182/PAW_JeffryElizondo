using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
    System.Threading.Tasks.Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
    System.Threading.Tasks.Task<UserRoleDTO?> GetUserRoleByIdAsync(decimal id);
}

public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;

    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async System.Threading.Tasks.Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var userRoles = await JsonProvider.DeserializeAsync<IEnumerable<UserRoleDTO>>(response);
        return userRoles;
    }

    public async System.Threading.Tasks.Task<UserRoleDTO?> GetUserRoleByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: id.ToString());
        var userRole = await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
        return userRole;
    }
}
