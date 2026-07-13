using MediatR;

namespace ProjectTracker.Application.Tasks.Queries;

public record GetAllTaskQuery(string UserId) : IRequest<IEnumerable<TaskItemDto>>;