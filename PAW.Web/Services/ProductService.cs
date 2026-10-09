using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<bool> CreateProductAsync(ProductDTO product);
    Task<bool> UpdateProductAsync(ProductDTO product);
    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: "");
        var products = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return products;
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var product = await JsonProvider.DeserializeAsync<ProductDTO>(response);
        return product;
    }

    public async Task<bool> CreateProductAsync(ProductDTO product)
    {
        var json = JsonSerializer.Serialize(product);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateProductAsync(ProductDTO product)
    {
        var json = JsonSerializer.Serialize(product);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
