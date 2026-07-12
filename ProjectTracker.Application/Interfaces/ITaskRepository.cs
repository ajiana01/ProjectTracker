using System.Threading;
using System.Threading.Tasks;
using ProjectTracker.Domain.Entities;

namespace ProjectTracker.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken);
}