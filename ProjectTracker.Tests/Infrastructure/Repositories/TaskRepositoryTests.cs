using Microsoft.EntityFrameworkCore;
using ProjectTracker.Domain.Entities;
using ProjectTracker.Infrastructure.Data;
using ProjectTracker.Infrastructure.Repositories;

namespace ProjectTracker.Tests.Infrastructure.Repositories;

public class TaskRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldSaveTaskToDatabase()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        using var context = new AppDbContext(options);
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