using MediatR;
using ProjectTracker.Application.Interfaces;

namespace ProjectTracker.Application.Tasks.Queries;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTaskQuery, IEnumerable<TaskItemDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItemDto>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllAsync(cancellationToken);
        return tasks.Where( t => t.UserId == request.UserId).Select( t => new TaskItemDto(
            t.Id,
            t.Title,
            t.Description,
            t.Status.ToString()
        ));
    }
}