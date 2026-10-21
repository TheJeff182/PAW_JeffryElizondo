using Microsoft.EntityFrameworkCore;
using PAW.MinimalAPI.Data;
using PAW.MinimalAPI.Models;

namespace PAW.MinimalAPI.Services;

/// <summary>
/// Repository interface for Task operations
/// </summary>
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(int id, TaskItem task);
    Task<bool> DeleteAsync(int id);
    Task<int> CountAsync();
}

/// <summary>
/// Entity Framework Core implementation of ITaskRepository using InMemory database
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        task.CreatedAt = DateTime.Now;
        task.LastModified = DateTime.Now;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem updatedTask)
    {
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null)
            return null;

        existingTask.Name = updatedTask.Name ?? existingTask.Name;
        existingTask.Description = updatedTask.Description ?? existingTask.Description;
        existingTask.Status = updatedTask.Status ?? existingTask.Status;
        existingTask.DueDate = updatedTask.DueDate ?? existingTask.DueDate;
        existingTask.LastModified = DateTime.Now;
        existingTask.ModifiedBy = updatedTask.ModifiedBy ?? existingTask.ModifiedBy;

        _context.Tasks.Update(existingTask);
        await _context.SaveChangesAsync();

        return existingTask;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> CountAsync()
    {
        return await _context.Tasks.CountAsync();
    }
}

