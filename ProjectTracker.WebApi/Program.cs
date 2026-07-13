using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Application.Interfaces;
using ProjectTracker.Application.Tasks.Commands;
using ProjectTracker.Infrastructure.Data;
using ProjectTracker.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=projecttracker.db"));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));

var app = builder.Build();

// hack: automatic create file db projecttracker.db when apps run first time
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// END POINT definition
app.MapPost("/api/tasks", async (CreateTaskCommand command, IMediator mediator) =>
{
    var taskId = await mediator.Send(command);
    return Results.Ok(taskId);
});

app.Run();


// expose program class for testing
public partial class Program {}

// public static class DbContextOptionsBuilderExtensions
// {
//     public static DbContextOptionsBuilder UseInMemoryDatabase(this DbContextOptionsBuilder optionsBuilder, string databaseName)
//     {
//         return optionsBuilder;
//     }
// }
