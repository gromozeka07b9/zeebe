using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using ServiceWorker.References;
using ServiceWorker.Infrastructure;
using BadHttpRequestException = Microsoft.AspNetCore.Server.Kestrel.Core.BadHttpRequestException;

string DemoProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StateChanger.bpmn");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var zeebeClient = ZeebeClient.Builder()
    .UseGatewayAddress("localhost:26500")
    .UsePlainText()
    .Build();
var topology = await zeebeClient.TopologyRequest().Send();

app.MapGet("/", () => "Zeebe test control api" +
                      "\nПубликация файла stateChanger.bpmn: POST /deploy" + 
                      "\nЗапуск нового процесса: POST /startProcess" + 
                      "\nПубликация сообщения: POST /publishMessage" + 
                      "\nСправочник исполнителей: GET /persons" + 
                      "\nСправочник типов работ: GET /workTypes" + 
                      "\nЗадача по id: GET /task/id" + 
                      "\nСписок задач: GET /tasks");

app.MapPost("/deploy", async () =>
{
    var deployResponse = await zeebeClient.NewDeployCommand()
        .AddResourceFile(DemoProcessPath)
        .Send();
    
    /*var processDefinitionKey = deployResponse.Processes[0].ProcessDefinitionKey;
 
    var processInstance = await zeebeClient
        .NewCreateProcessInstanceCommand()
        .ProcessDefinitionKey(processDefinitionKey)
        //.Variables(ProcessInstanceVariables)
        .Send();*/    
});

app.MapPost("/publishMessage", async (httpContext) =>
{
    StreamReader streamReader = new StreamReader(httpContext.Request.Body);
    string body = await streamReader.ReadToEndAsync();
    await zeebeClient.NewPublishMessageCommand()
        .MessageName("ExternalEvent")
        .CorrelationKey(Guid.NewGuid().ToString())
        .Variables(body)
        .Send();
});

app.MapPost("/call", async (HttpRequest httpContext) =>
{
    StreamReader streamReader = new StreamReader(httpContext.Body);
    string body = await streamReader.ReadToEndAsync();
    var zeebeResult = await zeebeClient.NewCreateProcessInstanceCommand().BpmnProcessId("StateChangerV1").LatestVersion().Variables(body).WithResult().Send();
    var result = JsonSerializer.Deserialize<ZeebeCommandResult>(zeebeResult.Variables);
    if (!result!.Success)
    {
        throw new Microsoft.AspNetCore.Http.BadHttpRequestException(result.Description);
    }
    else
    {
        //return Results.Json(new { NewId = "sdfsd" });
        return await Task.FromResult<string>(zeebeResult.Variables).ConfigureAwait(false);
    }

});

app.MapPost("/todoitems", async (string test) =>
{
    //db.Todos.Add(todo);
    //await db.SaveChangesAsync();
    return Results.Json(new { NewId = "sdfsd" });

//    return Results.Created($"/todoitems/{test}", test);
});

app.MapGet("/persons", () => JsonSerializer.Serialize(Persons.PossiblePersons));
app.MapGet("/workTypes", () => JsonSerializer.Serialize(WorkTypes.PossibleWorkTypes));
app.MapGet("/tasks", () =>
{
    var repository = new TaskRepository();
    repository.Filename = "../ServiceWorker/bin/Debug/net7.0/tasks.json";
    return JsonSerializer.Serialize(repository.GetAllTasks());
});
app.MapGet("/task/{taskId}", (string taskId) =>
{
    var repository = new TaskRepository();
    repository.Filename = "../ServiceWorker/bin/Debug/net7.0/tasks.json";
    return JsonSerializer.Serialize(repository.GetById(taskId));
});

app.Run();

public class ZeebeCommandResult
{
    public bool Success { get; set; }
    public string Description { get; set; }
}

