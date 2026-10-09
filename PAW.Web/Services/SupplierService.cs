using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<SupplierDTO?> GetSupplierByIdAsync(int id);
    Task<bool> CreateSupplierAsync(SupplierDTO supplierDTO);
    Task<bool> UpdateSupplierAsync(SupplierDTO supplierDTO);
    Task<bool> DeleteSupplierAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var suppliers = await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);
        return suppliers;
    }

    public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        var supplier = await JsonProvider.DeserializeAsync<SupplierDTO>(response);
        return supplier;
    }

    public async Task<bool> CreateSupplierAsync(SupplierDTO supplierDTO)
    {
        var json = JsonSerializer.Serialize(supplierDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateSupplierAsync(SupplierDTO supplierDTO)
    {
        var json = JsonSerializer.Serialize(supplierDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
