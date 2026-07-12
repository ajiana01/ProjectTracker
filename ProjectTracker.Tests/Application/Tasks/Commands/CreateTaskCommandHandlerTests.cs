using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectTracker.Application.Interfaces;
using ProjectTracker.Application.Tasks.Commands;
using ProjectTracker.Domain.Entities;
using Xunit;

namespace ProjectTracker.Tests.Application.Task.Commands;

public class CreateTaskCommandHandlerTest
{

    [Fact]
    public async System.Threading.Tasks.Task Handle_WithValidCommand_ShouldSaveTaskToRepository()
    {
        //Arrange
        var mockRepository = new Mock<ITaskRepository>();
        var command = new CreateTaskCommand("Setup Database", "IMplement EF Core");
        var handler = new CreateTaskCommandHandler(mockRepository.Object);

        //Act
        var resultId = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.NotEqual(Guid.Empty, resultId);
        //verify that method AddAsync only call once
        mockRepository.Verify(
            repo => repo.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        
    }
}