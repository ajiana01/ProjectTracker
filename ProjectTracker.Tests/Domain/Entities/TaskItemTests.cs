using System;
using ProjectTracker.Domain.Entities;
using Xunit;

namespace ProjectTracker.Tests.Domain.Entities;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateTaskItem()
    {
        //ARRANGE
        var title = "Setup CI/CD Pipeline";
        var description = "Configure GitHub Actions for the backend";
        var userId = "dummy-user-id";

        //ACT
        var task = new TaskItem(title, description, userId);

        //ASSERT
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);

        //Status
        Assert.Equal(ProjectTracker.Domain.Enums.TaskStatus.ToDo, task.Status);

    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithEmptyTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        var description = "this should fail";
        var userId = "dummy-user-id";

        // Use null-forgiving operator to satisfy nullable warnings in tests where null is expected
        Assert.Throws<ArgumentException>(() => new TaskItem(invalidTitle!, description, userId));
    }
}