using Microsoft.EntityFrameworkCore;
using PAW.MinimalAPI.Data;
using PAW.MinimalAPI.Extensions;
using PAW.MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios necesarios para la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la base de datos en memoria con Entity Framework Core
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseInMemoryDatabase("TasksDatabase"));

// Registrar el repositorio de tareas con inyección de dependencias
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Configurar CORS para permitir solicitudes desde PAW.Web
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Inicializar la base de datos en memoria con los datos de prueba
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configurar la aplicación según el entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC");

// Registrar los endpoints donde la API expone las funciones de tarea
app.MapTaskEndpoints();

app.Run();


