using System;
using MediatR;

namespace ProjectTracker.Application.Tasks.Commands;

public record CreateTaskCommand(string Title, string Description, string UserId = ""): IRequest<Guid>;