using ServiceWorker.Infrastructure;
using StateChanger.Infrastructure;

namespace ServiceWorker.Commands;

public class TaskCustomCommands
{
    private readonly TaskRepository taskRepository;
    private enum TaskStates
    {
        Scheduled, InProgress, Finished, Canceled         
    }

    public TaskCustomCommands()
    {
        taskRepository = new TaskRepository();
    }
    
    public void CustomCommand(string id, string commandName, string state, string workType, string person, string cancelReasonText)
    {
        Console.WriteLine($"Custom command [{commandName}] executed, id [{id}]");
        //taskRepository.AddOrUpdate(id, state, workType, person, cancelReasonText);
    }

}