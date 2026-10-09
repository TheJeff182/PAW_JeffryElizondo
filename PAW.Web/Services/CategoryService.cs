using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CategoryDTO category);
    Task<bool> UpdateCategoryAsync(CategoryDTO category);
    Task<bool> DeleteCategoryAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var categories = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        return categories;
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var category = await JsonProvider.DeserializeAsync<CategoryDTO>(response);
        return category;
    }

    public async Task<bool> CreateCategoryAsync(CategoryDTO category)
    {
        var json = JsonSerializer.Serialize(category);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateCategoryAsync(CategoryDTO category)
    {
        var json = JsonSerializer.Serialize(category);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}

