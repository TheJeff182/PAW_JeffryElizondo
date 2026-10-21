using Microsoft.EntityFrameworkCore;
using PAW.MinimalAPI.Data;
using PAW.MinimalAPI.Models;

namespace PAW.MinimalAPI.Services;

/// <summary>
/// Interfaz que define las operaciones que se pueden hacer con las tareas
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
/// Implementación del repositorio de tareas usando Entity Framework Core
/// Almacena y recupera tareas de la base de datos en memoria
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las tareas ordenadas por fecha de creación (más recientes primero)
    /// </summary>
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    /// <summary>
    /// Busca una tarea específica por su ID
    /// </summary>
    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    /// <summary>
    /// Crea una nueva tarea en la base de datos
    /// </summary>
    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        task.CreatedAt = DateTime.Now;
        task.LastModified = DateTime.Now;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    /// <summary>
    /// Actualiza una tarea existente
    /// </summary>
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

    /// <summary>
    /// Elimina una tarea de la base de datos
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Cuenta el total de tareas en la base de datos
    /// </summary>
    public async Task<int> CountAsync()
    {
        return await _context.Tasks.CountAsync();
    }
}


