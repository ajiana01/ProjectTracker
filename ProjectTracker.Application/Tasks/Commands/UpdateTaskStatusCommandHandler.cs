using MediatR;
using ProjectTracker.Application.Interfaces;

namespace ProjectTracker.Application.Tasks.Commands;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if(task == null || task.UserId != request.UserId)
        {
            return false;
        }

        if (Enum.TryParse<Domain.Enums.TaskStatus>(request.Status, true, out var parsedStatus))
        {
            task.UpdateStatus(parsedStatus);
            await _taskRepository.UpdateAsync(task, cancellationToken);
            return true;
        }
        return false;
    }
}