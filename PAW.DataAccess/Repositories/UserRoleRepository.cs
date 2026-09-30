using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    System.Threading.Tasks.Task<bool> UpsertAsync(UserRole entity, bool isUpdating);
    System.Threading.Tasks.Task<bool> CreateAsync(UserRole entity);
    System.Threading.Tasks.Task<bool> DeleteAsync(UserRole entity);
    System.Threading.Tasks.Task<IEnumerable<UserRole>> ReadAsync();
    System.Threading.Tasks.Task<UserRole> FindAsync(int id);
    System.Threading.Tasks.Task<bool> UpdateAsync(UserRole entity);
    System.Threading.Tasks.Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);
    System.Threading.Tasks.Task<bool> ExistsAsync(UserRole entity);
}

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
}
