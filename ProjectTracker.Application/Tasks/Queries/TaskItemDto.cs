namespace ProjectTracker.Application.Tasks.Queries;

public record TaskItemDto(Guid Id, string Title, string Description, string Status);