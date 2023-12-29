namespace StateChanger.Infrastructure;

public class BusinessTask
{
    public string Id { get; set; }
    public int Number { get; set; }
    public string WorkTypeId { get; set; }
    public string PersonId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PauseDate { get; set; }
    public DateTime ResumeDate { get; set; }
    public DateTime AssignDate { get; set; }
    public List<string> History { get; set; }

}