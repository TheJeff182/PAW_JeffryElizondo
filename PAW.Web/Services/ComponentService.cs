using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
    Task<ComponentDTO> GetComponentByIdAsync(decimal id);
    Task<bool> CreateComponentAsync(ComponentDTO component);
    Task<bool> UpdateComponentAsync(ComponentDTO component);
    Task<bool> DeleteComponentAsync(decimal id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var components = await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(response);
        return components;
    }

    public async Task<ComponentDTO> GetComponentByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var component = await JsonProvider.DeserializeAsync<ComponentDTO>(response);
        return component;
    }

    public async Task<bool> CreateComponentAsync(ComponentDTO component)
    {
        var json = JsonSerializer.Serialize(component);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateComponentAsync(ComponentDTO component)
    {
        var json = JsonSerializer.Serialize(component);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteComponentAsync(decimal id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
