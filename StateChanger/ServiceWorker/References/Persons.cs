namespace ServiceWorker.References;

public class Persons
{
    public string Id { get; set; }
    public string Name { get; set; }

    public static List<Persons> PossiblePersons = new List<Persons>()
    {
        new Persons() { Id = "user1", Name = "Ivanych" },
        new Persons() { Id = "user2", Name = "Petrovich" },
        new Persons() { Id = "user3", Name = "Hulio" }
    };
}

