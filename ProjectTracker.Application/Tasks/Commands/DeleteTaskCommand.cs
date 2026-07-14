using MediatR;

namespace ProjectTracker.Application.Tasks.Commands;

public record DeleteTaskCommand(Guid Id, string UserId = "") : IRequest<bool>;