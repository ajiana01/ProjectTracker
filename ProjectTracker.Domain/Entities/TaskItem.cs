namespace ProjectTracker.Domain.Entities;

public class TaskItem
{
    public Guid Id {get; private set;}
    public string Title {get; private set;}
    public string Description {get; private set;}
    public Enums.TaskStatus Status {get; private set;}
    public string UserId {get; private set;}

    protected TaskItem()
    {
        Id = Guid.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Status = Enums.TaskStatus.ToDo;
        UserId = string.Empty;
    }

    public TaskItem(string title, string description, string userId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tittle cannot be empty.", nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Status = Enums.TaskStatus.ToDo;
        UserId = userId;
    }

    public void UpdateStatus(Enums.TaskStatus newStatus)
    {
        Status = newStatus;
    }
}