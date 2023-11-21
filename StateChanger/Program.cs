// See https://aka.ms/new-console-template for more information

using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using StateChanger.Commands;
using StateChanger.Infrastructure;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

Console.WriteLine("Zeebe worker started");

/*TaskCommands taskCommands = new TaskCommands();
taskCommands.Schedule("go1", "user1");
taskCommands.Cancel("098ebe4e-4688-46d3-9ac1-c574565e6dee", "not working");
taskCommands.Start("61f95a31-9fed-43be-bd3b-284191882fd7");
taskCommands.Finish("61f95a31-9fed-43be-bd3b-284191882fd7");
return;*/
var zeebeClient = ZeebeClient.Builder()
    .UseGatewayAddress("localhost:26500")
    .UsePlainText()
    .Build();

var topology = await zeebeClient.TopologyRequest().Send();

using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
{
    zeebeClient.NewWorker()
        .JobType("TaskCommand")
        .Handler(HandleTaskCommandJob)
        .MaxJobsActive(5)
        .Name("ChangeState")
        .AutoCompletion()
        .PollInterval(TimeSpan.FromSeconds(1))
        .Timeout(TimeSpan.FromSeconds(10))
        .Open();
 
    // blocks main thread, so that worker can run
    signal.WaitOne();
}

Console.ReadLine();

static void HandleTaskCommandJob(IJobClient jobClient, IJob job)
{
    var taskCommands = new TaskCommands();
    var jobKey = job.Key;
    Console.WriteLine("Handling job: " + job);
    var command = JsonSerializer.Deserialize<CommandDescription>(job.Variables);
    CommandResult commandResult = null;
    switch (command?.Command)
    {
        case "Schedule":
        {
            commandResult = taskCommands.Schedule(command.WorkType, command.Person);
        };break;
        case "Start":
        {
            commandResult = taskCommands.Start(command.Id);
        };break;
        case "Finish":
        {
            commandResult = taskCommands.Finish(command.Id);
        };break;
        case "Cancel":
        {
            commandResult = taskCommands.Cancel(command.Id, command.CancelReasonText);
        };break;
    }
    jobClient.NewCompleteJobCommand(jobKey)
        .Variables(JsonSerializer.Serialize(commandResult))
        .Send()
        .GetAwaiter()
        .GetResult();
 
    /*if (jobKey % 3 == 0)
    {
        jobClient.NewCompleteJobCommand(jobKey)
            .Variables("{\"foo\":2}")
            .Send()
            .GetAwaiter()
            .GetResult();
    }
    else if (jobKey % 2 == 0)
    {
        jobClient.NewFailCommand(jobKey)
            .Retries(job.Retries - 1)
            .ErrorMessage("Example fail")
            .Send()
            .GetAwaiter()
            .GetResult();
    }
    else
    {
        // auto completion
    }*/
}

public class CommandDescription
{
    public string Id { get; set; }
    public string Command { get; set; }
    public string WorkType { get; set; }
    public string Person { get; set; }
    public string CancelReasonText { get; set; }
}