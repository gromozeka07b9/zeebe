namespace ServiceWorker.References;

public class WorkTypes
{
    public string Id { get; set; }
    public string Name { get; set; }

    public static List<WorkTypes> PossibleWorkTypes = new List<WorkTypes>()
    {
        new WorkTypes() { Id = "work1", Name = "Dig" },
        new WorkTypes() { Id = "work2", Name = "Throw something" },
        new WorkTypes() { Id = "work3", Name = "Sleep" },
        new WorkTypes() { Id = "work4", Name = "Procrastinate" },
        new WorkTypes() { Id = "work5", Name = "Programming" }
    };
}

