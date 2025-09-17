using GrainInterfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class UserGrain : Grain, IUser
{
    private readonly ILogger _logger;

    private Agent _agent;

    public UserGrain(ILogger<UserGrain> logger)
    {
        _logger = logger;
        _agent = new("test", "Test Name", 0);
    }
    public ValueTask<string> DebugDump()
    {
        return ValueTask.FromResult(this.GetPrimaryKey().ToString());
    }

    public ValueTask<List<string>> AvailibleTasks()
    {
        return ValueTask.FromResult(new List<string> {"testTaskId"});
    }
    public async ValueTask<string> StartTask(string taskId)
    {
        _logger.LogInformation("Starting task {TaskId}", taskId);
        bool ret = await GrainFactory.GetGrain<ITask>(taskId).StartTask(_agent.Id);
        return $"Started task {taskId}: {ret}";
    }
    public ValueTask<Agent> GetAgentInfo()
    {
        return ValueTask.FromResult(_agent);
    }

    ValueTask<string> IUser.SayHello(string greeting)
    {
        _logger.LogInformation("""
            SayHello message received: greeting = "{Greeting}"
            """,
            greeting);
        
        return ValueTask.FromResult($"""

            Client said: "{greeting}", so HelloGrain says: Hello!
            """);
    }
}