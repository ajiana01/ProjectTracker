using MediatR;

namespace ProjectTracker.Application.Tasks.Commands;

public record UpdateTaskStatusCommand(string Status, Guid Id = default, string UserId = ""): IRequest<bool>;