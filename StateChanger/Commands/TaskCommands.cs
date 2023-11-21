using StateChanger.Infrastructure;

namespace StateChanger.Commands;

public class TaskCommands
{
    private readonly TaskRepository taskRepository;
    public TaskCommands()
    {
        taskRepository = new TaskRepository();
    }
    
    public CommandResult Schedule(string workType, string person)
    {
        string newId = Guid.NewGuid().ToString();
        var newState = TaskStates.Scheduled;
        taskRepository.AddOrUpdate(newId, newState.ToString(), workType, person);
        Console.WriteLine($"New task id:{newId}");
        return new CommandResult(){TaskId = newId, Status = newState};
    }
    public CommandResult Start(string id)
    {
        var newState = TaskStates.InProgress;
        taskRepository.AddOrUpdate(id, newState.ToString());
        return new CommandResult(){TaskId = id, Status = newState};
    }
    public CommandResult Finish(string id)
    {
        var newState = TaskStates.Finished;
        taskRepository.AddOrUpdate(id, newState.ToString());
        return new CommandResult(){TaskId = id, Status = newState};
    }
    public CommandResult Cancel(string id, string cancelReasonText)
    {
        var newState = TaskStates.Canceled;
        taskRepository.AddOrUpdate(id, newState.ToString(), null , null , cancelReasonText);
        return new CommandResult(){TaskId = id, Status = newState};
    }

}

public enum TaskStates
{
    Scheduled, InProgress, Finished, Canceled         
}

public class CommandResult
{
    public string TaskId { get; set; }
    public TaskStates Status { get; set; }
}