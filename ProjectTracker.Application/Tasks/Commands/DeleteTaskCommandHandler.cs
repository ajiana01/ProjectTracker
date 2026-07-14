using MediatR;
using ProjectTracker.Application.Interfaces;

namespace ProjectTracker.Application.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task == null || task.UserId != request.UserId ) 
        {
            return false;
        }
        
        await _taskRepository.DeleteAsync(task, cancellationToken);

        return true;
    }
}