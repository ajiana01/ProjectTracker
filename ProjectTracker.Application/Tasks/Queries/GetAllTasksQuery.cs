using MediatR;

namespace ProjectTracker.Application.Tasks.Queries;

public record GetAllTaskQuery : IRequest<IEnumerable<TaskItemDto>>;