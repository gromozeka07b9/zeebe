namespace StateChanger.Infrastructure;

public class Task
{
    public string Id { get; set; }
    public string WorkType { get; set; }
    public string Person { get; set; }
    public string State { get; set; }
    public string CancelReasonText { get; set; }
    public List<string> History { get; set; }

}