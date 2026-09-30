using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserActionRepository : IRepositoryBase<UserAction>
{
    System.Threading.Tasks.Task<bool> UpsertAsync(UserAction entity, bool isUpdating);
    System.Threading.Tasks.Task<bool> CreateAsync(UserAction entity);
    System.Threading.Tasks.Task<bool> DeleteAsync(UserAction entity);
    System.Threading.Tasks.Task<IEnumerable<UserAction>> ReadAsync();
    System.Threading.Tasks.Task<UserAction> FindAsync(int id);
    System.Threading.Tasks.Task<bool> UpdateAsync(UserAction entity);
    System.Threading.Tasks.Task<bool> UpdateManyAsync(IEnumerable<UserAction> entities);
    System.Threading.Tasks.Task<bool> ExistsAsync(UserAction entity);
}

public class UserActionRepository : RepositoryBase<UserAction>, IUserActionRepository
{
}
