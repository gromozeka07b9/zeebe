using ServiceWorker.Infrastructure;
using ServiceWorker.References;
using StateChanger.Infrastructure;

namespace StateChanger.Commands;

public class TaskCommands
{
    private readonly TaskRepository taskRepository;
    public TaskCommands()
    {
        taskRepository = new TaskRepository();
    }
    
    public CommandResult Schedule(string workTypeId)
    {
        var isWorkTypeExists = WorkTypes.PossibleWorkTypes.Exists(w=> w.Id == workTypeId);
        if(!isWorkTypeExists) 
            return new CommandResult() { Success = false };

        string newId = Guid.NewGuid().ToString();
        int newNumber = taskRepository.GetNextNewNumber();
        bool result = taskRepository.AddOrUpdate(newId, TaskRepository.TaskStates.Scheduled.ToString(), number: newNumber, workTypeId: workTypeId, createDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(newId)};
        }
        
        return new CommandResult() { Success = false };
    }
    public CommandResult Start(string id)
    {
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.InProgress.ToString(), startDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }
    public CommandResult Finish(string id)
    {
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.Finished.ToString(), endDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }
    public CommandResult Cancel(string id, string description)
    {
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.Cancelled.ToString(), description: description, endDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }

    public CommandResult Pause(string id, string description)
    {
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.Paused.ToString(), description: description, pauseDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }

    public CommandResult Resume(string id)
    {
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.InProgress.ToString(), resumeDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }

    public CommandResult Assign(string id, string personId)
    {
        //проверка на справочник исполнителей
        bool result = taskRepository.AddOrUpdate(id, TaskRepository.TaskStates.Assigned.ToString(), personId: personId, assignDate: DateTime.Now);
        if (result)
        {
            return new CommandResult(){Success = true, ResultTask = taskRepository.GetById(id)};
        }
        return new CommandResult() { Success = false };
    }

}

public class CommandResult
{
    public bool Success { get; set; }
    public TaskRepository.TaskStates? Status { get; set; }
    public BusinessTask? ResultTask { get; set; } = null;
}