using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Grains;

public class TaskGrain : Grain, ITask, IRemindable
{
    private readonly ILogger _logger;

    private Agent _agent;
    private bool running;

    public TaskGrain(ILogger<HelloGrain> logger)
    {
        _logger = logger;
        _agent = new("test", "Test Name", 0);
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<List<string>> getAvailibleTasks()
    {
        return ValueTask.FromResult(new List<string> { "testTaskId" });
    }

    public ValueTask<bool> StartTask(string agentId)
    {
        throw new NotImplementedException();

    }

    public ValueTask<string> Status()
    {
        throw new NotImplementedException();
    }
    Task IRemindable.ReceiveReminder(string reminderName, TickStatus status)
    {
        Console.WriteLine("Thanks for reminding me-- I almost forgot!");
        return Task.CompletedTask;
    }
}