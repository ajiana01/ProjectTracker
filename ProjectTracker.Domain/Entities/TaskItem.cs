using System;
using ProjectTracker.Domain.Enums;

namespace ProjectTracker.Domain.Entities;

public class TaskItem
{
    public Guid Id {get; private set;}
    public string Title {get; private set;}
    public string Description {get; private set;}
    public Enums.TaskStatus Status {get; private set;}

    public TaskItem(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tittle cannot be empty.", nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Status = ProjectTracker.Domain.Enums.TaskStatus.ToDo;
    }
}