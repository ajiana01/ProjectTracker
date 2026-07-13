using MediatR;
using ProjectTracker.Application.Interfaces;
using ProjectTracker.Domain.Entities;

namespace ProjectTracker.Application.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(request.Title, request.Description, request.UserId);

        await _taskRepository.AddAsync(task, cancellationToken);

        return task.Id;
    }
}