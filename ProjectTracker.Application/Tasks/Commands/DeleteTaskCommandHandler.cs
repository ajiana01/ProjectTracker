using MediatR;
using ProjectTracker.Application.Interfaces;

namespace ProjectTracker.Application.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        Console.WriteLine("\n================ [ DEBUG IDOR ] ================");
        Console.WriteLine($"[1] Target Task ID yang dicari : {request.Id}");
        Console.WriteLine($"[2] UserId dari Token JWT Anda : '{request.UserId}'");

        if (task == null) 
        {
            Console.WriteLine("[!] HASIL: GAGAL (Task tidak ditemukan di PostgreSQL)");
            Console.WriteLine("================================================\n");
            return false;
        }

        Console.WriteLine($"[3] UserId dari Task di Database : '{task.UserId}'");

        if (task.UserId != request.UserId) 
        {
            Console.WriteLine("[!] HASIL: GAGAL (UserId tidak sama / Dianggap Hacker!)");
            Console.WriteLine("================================================\n");
            return false;
        }

        Console.WriteLine("[V] HASIL: SUKSES (Mengeksekusi SQL DELETE...)");
        Console.WriteLine("================================================\n");
        
        await _taskRepository.DeleteAsync(task, cancellationToken);

        return true;
    }
}