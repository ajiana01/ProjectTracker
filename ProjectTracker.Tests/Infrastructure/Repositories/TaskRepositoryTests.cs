using Microsoft.EntityFrameworkCore;
using ProjectTracker.Domain.Entities;
using ProjectTracker.Infrastructure.Data;
using ProjectTracker.Infrastructure.Repositories;

namespace ProjectTracker.Tests.Infrastructure.Repositories;

public class TaskRepositoryTests
{
    [Fact(Skip = "Issue dengan EF Core InMemory di .NET 10.")]
    public async Task AddAsync_ShouldSaveTaskToDatabase()
    {
        // Arrange
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        
        using var context = new AppDbContext(builder.Options);
        var repository = new TaskRepository(context);
        var task = new TaskItem("Test Integration", "Testing EF Core Repository");

        //Act
        await repository.AddAsync(task, CancellationToken.None);

        //Assert
        var savedTask = await context.TaskItems.FirstOrDefaultAsync(t => t.Id == task.Id);

        Assert.NotNull(savedTask);
        Assert.Equal("Test Integration", savedTask.Title);
    }
}