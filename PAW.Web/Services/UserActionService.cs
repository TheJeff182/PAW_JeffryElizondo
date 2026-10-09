using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    System.Threading.Tasks.Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
    System.Threading.Tasks.Task<UserActionDTO?> GetUserActionByIdAsync(decimal id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async System.Threading.Tasks.Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var userActions = await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);
        return userActions;
    }

    public async System.Threading.Tasks.Task<UserActionDTO?> GetUserActionByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        var userAction = await JsonProvider.DeserializeAsync<UserActionDTO>(response);
        return userAction;
    }
}
