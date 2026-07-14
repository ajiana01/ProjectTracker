using Microsoft.EntityFrameworkCore;
using ProjectTracker.Application.Interfaces;
using ProjectTracker.Domain.Entities;
using ProjectTracker.Infrastructure.Data;

namespace ProjectTracker.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await _context.TaskItems.AddAsync(task, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.TaskItems.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _context.TaskItems.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}