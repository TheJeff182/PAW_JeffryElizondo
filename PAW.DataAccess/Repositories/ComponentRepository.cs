using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IComponentRepository : IRepositoryBase<Component>
{
    Task<bool> UpsertAsync(Component entity, bool isUpdating);
    Task<bool> CreateAsync(Component entity);
    Task<bool> DeleteAsync(Component entity);
    Task<IEnumerable<Component>> ReadAsync();
    Task<bool> UpdateAsync(Component entity);
    Task<bool> UpdateManyAsync(IEnumerable<Component> entities);
    Task<bool> ExistsAsync(Component entity);
    Task<Component?> FindByDecimalIdAsync(decimal id);
}

public class ComponentRepository : RepositoryBase<Component>, IComponentRepository
{
    public async Task<Component?> FindByDecimalIdAsync(decimal id)
    {
        return await DbContext.Set<Component>().FindAsync(id);
    }
}
