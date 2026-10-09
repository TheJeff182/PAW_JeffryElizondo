using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
    Task<InventoryDTO?> GetInventoryByIdAsync(int id);
    Task<bool> CreateInventoryAsync(InventoryDTO inventoryDTO);
    Task<bool> UpdateInventoryAsync(InventoryDTO inventoryDTO);
    Task<bool> DeleteInventoryAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var inventories = await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);
        return inventories;
    }

    public async Task<InventoryDTO?> GetInventoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var inventory = await JsonProvider.DeserializeAsync<InventoryDTO>(response);
        return inventory;
    }

    public async Task<bool> CreateInventoryAsync(InventoryDTO inventoryDTO)
    {
        inventoryDTO.InventoryId = 0;
        var json = JsonSerializer.Serialize(inventoryDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateInventoryAsync(InventoryDTO inventoryDTO)
    {
        var json = JsonSerializer.Serialize(inventoryDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteInventoryAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}

