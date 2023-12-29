using System.Text.Json;
using StateChanger.Infrastructure;

namespace ServiceWorker.Infrastructure;

public class TaskRepository
{
    private string filename = "tasks.json";
    public enum TaskStates
    {
        Scheduled, InProgress, Finished, Cancelled, Assigned, Paused         
    }

    public bool AddOrUpdate(string id, string state, int? number = default, string? workTypeId = null, string? personId = null, string? description = null, DateTime? createDate = null, DateTime? startDate = null, DateTime? endDate = null, DateTime? pauseDate = null, DateTime? resumeDate = null, DateTime? assignDate = null)
    {
        List<BusinessTask>? tasks = GetTasksFromFile();

        var task = tasks?.SingleOrDefault(t => t.Id == id);
        if (task == default)
        {
            task = new BusinessTask()
            {
                Id = id, State = state,
                History = new List<string>() { state }
            };
            tasks?.Add(task);
        }
        else
        {
            task.State = state;
            task.History.Add(state);
        }
        if(number.HasValue) task.Number = number.Value;
        if(workTypeId is not null) task.WorkTypeId = workTypeId;
        if(personId is not null) task.PersonId = personId;
        if(description is not null) task.Description = description;
        if (createDate.HasValue) task.CreateDate = createDate.Value;
        if (startDate.HasValue) task.StartDate = startDate.Value;
        if (endDate.HasValue) task.EndDate = endDate.Value;
        if (pauseDate.HasValue) task.PauseDate = pauseDate.Value;
        if (resumeDate.HasValue) task.ResumeDate = resumeDate.Value;
        if (assignDate.HasValue) task.AssignDate = assignDate.Value;

        string jsonUpdated = JsonSerializer.Serialize(tasks);
        File.WriteAllText(filename, jsonUpdated);
        return true;
    }

    private List<BusinessTask>? GetTasksFromFile()
    {
        List<BusinessTask>? tasks = new List<BusinessTask>();
        try
        {
            string json = File.ReadAllText(Filename);
            tasks = JsonSerializer.Deserialize<List<BusinessTask>>(json);
        }
        catch (FileNotFoundException)
        {
        }

        return tasks;
    }

    public List<BusinessTask>? GetAllTasks()
    {
        List<BusinessTask>? tasks = GetTasksFromFile();
        return tasks ?? default;
    }
    
    public BusinessTask? GetById(string id)
    {
        List<BusinessTask>? tasks = GetTasksFromFile();
        return tasks?.FirstOrDefault(t => t.Id == id) ?? default;
    }

    public int GetNextNewNumber()
    {
        List<BusinessTask>? tasks = GetTasksFromFile();
        if (tasks == default || !tasks.Any()) return 1;
        var max = tasks?.AsQueryable().Max(t=>t.Number) ?? 1;
        return ++max;
    }

    public string Filename
    {
        get => filename;
        set => filename = value;
    }
}