using System.Text.Json;

namespace StateChanger.Infrastructure;

public class TaskRepository
{
    private const string filename = "tasks.json";
    
    public void AddOrUpdate(string id, string state, string? workType = null, string? person = null, string? cancelReasonText = null)
    {
        List<Task>? tasks = new List<Task>();
        try
        {
            string json = File.ReadAllText(filename);
            tasks = JsonSerializer.Deserialize<List<Task>>(json);
        }
        catch (FileNotFoundException)
        {
        }

        var task = tasks?.SingleOrDefault(t => t.Id == id);
        if (task == default)
        {
            tasks?.Add(new Task()
            {
                Id = id, Person = person, State = state, WorkType = workType, CancelReasonText = cancelReasonText,
                History = new List<string>(){state}
            });
        }
        else
        {
            task.State = state;
            task.History.Add(state);
            if(workType is not null) task.WorkType = workType;
            if(person is not null) task.Person = person;
            if(cancelReasonText is not null) task.CancelReasonText = cancelReasonText;
        }
        string jsonUpdated = JsonSerializer.Serialize(tasks);
        File.WriteAllText(filename, jsonUpdated);
    }

}