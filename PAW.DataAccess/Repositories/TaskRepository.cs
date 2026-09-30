using PAW.Models;
using PAW.Repositories;
using Task = PAW.Models.Task;

namespace PAW.DataAccess.Repositories;

public interface ITaskRepository : IRepositoryBase<Task>
{
    System.Threading.Tasks.Task<bool> UpsertAsync(Task entity, bool isUpdating);
    System.Threading.Tasks.Task<bool> CreateAsync(Task entity);
    System.Threading.Tasks.Task<bool> DeleteAsync(Task entity);
    System.Threading.Tasks.Task<IEnumerable<Task>> ReadAsync();
    System.Threading.Tasks.Task<Task> FindAsync(int id);
    System.Threading.Tasks.Task<bool> UpdateAsync(Task entity);
    System.Threading.Tasks.Task<bool> UpdateManyAsync(IEnumerable<Task> entities);
    System.Threading.Tasks.Task<bool> ExistsAsync(Task entity);
}

public class TaskRepository : RepositoryBase<Task>, ITaskRepository
{
}
