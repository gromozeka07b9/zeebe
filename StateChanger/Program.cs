// See https://aka.ms/new-console-template for more information

using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

Console.WriteLine("Zeebe worker started");
var zeebeClient = ZeebeClient.Builder()
    .UseGatewayAddress("localhost:26500")
    .UsePlainText()
    .Build();

var topology = await zeebeClient.TopologyRequest().Send();
var test = topology.Brokers;

using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
{
    zeebeClient.NewWorker()
        .JobType("ChangeState")
        .Handler(HandleJob)
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

static void HandleJob(IJobClient jobClient, IJob job)
{
    // business logic
    var jobKey = job.Key;
    Console.WriteLine("Handling job: " + job);
 
    if (jobKey % 3 == 0)
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
    }
}