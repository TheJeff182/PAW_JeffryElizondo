using Microsoft.EntityFrameworkCore;
using PAW.MinimalAPI.Models;

namespace PAW.MinimalAPI.Data;

/// <summary>
/// Contexto de la base de datos en memoria para gestionar tareas
/// Usa Entity Framework Core con almacenamiento en memoria
/// </summary>
public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar la entidad TaskItem
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        // Agregar datos de prueba iniciales
        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 1,
                Name = "Implementar Minimal API",
                Description = "Crear la estructura base del Minimal API para Tasks",
                Status = "In Progress",
                DueDate = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now.AddDays(-5),
                LastModified = DateTime.Now,
                ModifiedBy = "System"
            },
            new TaskItem
            {
                Id = 2,
                Name = "Integrar con MVC",
                Description = "Consumir el Minimal API desde PAW.Web",
                Status = "Pending",
                DueDate = DateTime.Now.AddDays(14),
                CreatedAt = DateTime.Now.AddDays(-3),
                LastModified = DateTime.Now,
                ModifiedBy = "System"
            }
        );
    }
}
