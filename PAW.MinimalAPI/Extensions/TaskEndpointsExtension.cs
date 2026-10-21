using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PAW.MinimalAPI.Models;
using PAW.MinimalAPI.Services;

namespace PAW.MinimalAPI.Extensions;

/// <summary>
/// Extensión que registra todos los endpoints de tareas en el Minimal API
/// Aquí se definen las rutas GET, POST, PUT y DELETE para gestionar tareas
/// </summary>
public static class TaskEndpointsExtension
{
    /// <summary>
    /// Registra todos los endpoints de tareas en el grupo /api/tasks
    /// </summary>
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks")
            .WithName("Tasks")
            .WithOpenApi();

        group.MapGet("/", GetAllTasks)
            .WithName("GetAllTasks")
            .WithOpenApi()
            .Produces<List<TaskItem>>(StatusCodes.Status200OK)
            .WithDescription("Obtiene todas las tareas");

        group.MapGet("/{id}", GetTaskById)
            .WithName("GetTaskById")
            .WithOpenApi()
            .Produces<TaskItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Obtiene una tarea específica por su ID");

        group.MapPost("/", CreateTask)
            .WithName("CreateTask")
            .WithOpenApi()
            .Produces<TaskItem>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithDescription("Crea una nueva tarea");

        group.MapPut("/{id}", UpdateTask)
            .WithName("UpdateTask")
            .WithOpenApi()
            .Produces<TaskItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithDescription("Actualiza una tarea existente");

        group.MapDelete("/{id}", DeleteTask)
            .WithName("DeleteTask")
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Elimina una tarea");
    }

    /// <summary>
    /// Obtiene la lista completa de todas las tareas
    /// </summary>
    private static async Task<IResult> GetAllTasks(ITaskRepository repository)
    {
        var tasks = await repository.GetAllAsync();
        return Results.Ok(tasks);
    }

    /// <summary>
    /// Obtiene una tarea específica por su ID
    /// </summary>
    private static async Task<IResult> GetTaskById(int id, ITaskRepository repository)
    {
        var task = await repository.GetByIdAsync(id);
        if (task == null)
            return Results.NotFound($"No se encontró la tarea con ID {id}");

        return Results.Ok(task);
    }

    /// <summary>
    /// Crea una nueva tarea en la base de datos
    /// </summary>
    private static async Task<IResult> CreateTask(CreateTaskDto dto, ITaskRepository repository)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Results.BadRequest(new ProblemDetails 
            { 
                Title = "Error de validación",
                Detail = "El nombre de la tarea es obligatorio"
            });

        var task = new TaskItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status ?? "Pending",
            DueDate = dto.DueDate,
            ModifiedBy = "System"
        };

        var createdTask = await repository.CreateAsync(task);
        return Results.Created($"/api/tasks/{createdTask.Id}", createdTask);
    }

    /// <summary>
    /// Actualiza una tarea existente
    /// </summary>
    private static async Task<IResult> UpdateTask(int id, CreateTaskDto dto, ITaskRepository repository)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Error de validación",
                Detail = "El nombre de la tarea es obligatorio"
            });

        var task = new TaskItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            ModifiedBy = "System"
        };

        var updatedTask = await repository.UpdateAsync(id, task);
        if (updatedTask == null)
            return Results.NotFound($"No se encontró la tarea con ID {id}");

        return Results.Ok(updatedTask);
    }

    /// <summary>
    /// Elimina una tarea de la base de datos
    /// </summary>
    private static async Task<IResult> DeleteTask(int id, ITaskRepository repository)
    {
        var deleted = await repository.DeleteAsync(id);
        if (!deleted)
            return Results.NotFound($"No se encontró la tarea con ID {id}");

        return Results.NoContent();
    }
}

/// <summary>
/// Modelo de transferencia de datos para crear y actualizar tareas
/// Se utiliza para recibir los datos del cliente en las solicitudes POST y PUT
/// </summary>
public class CreateTaskDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
}

