using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int id);
    Task<bool> CreateUserAsync(UserDTO userDTO);
    Task<bool> UpdateUserAsync(UserDTO userDTO);
    Task<bool> DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var users = await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);
        return users;
    }

    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        var user = await JsonProvider.DeserializeAsync<UserDTO>(response);
        return user;
    }

    public async Task<bool> CreateUserAsync(UserDTO userDTO)
    {
        var json = JsonSerializer.Serialize(userDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateUserAsync(UserDTO userDTO)
    {
        var json = JsonSerializer.Serialize(userDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
