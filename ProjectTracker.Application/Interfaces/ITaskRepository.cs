
using ProjectTracker.Domain.Entities;

namespace ProjectTracker.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken);

    Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken);
}