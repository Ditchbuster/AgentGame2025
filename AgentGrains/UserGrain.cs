using System.ComponentModel;
using GrainInterfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class UserGrain : Grain, IUser
{
    private readonly ILogger _logger;
    private readonly IPersistentState<UserState> _userState;

    public UserGrain(ILogger<UserGrain> logger, [PersistentState("userState")] IPersistentState<UserState> userState)
    {
        _logger = logger;
        _userState = userState;

    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userState.State.AgentId))
        {
            _userState.State.AgentId = this.GetPrimaryKeyString();
            _userState.WriteStateAsync(cancellationToken);
        }
        return base.OnActivateAsync(cancellationToken);
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
        bool ret = await GrainFactory.GetGrain<ITask>(taskId).StartTask(_userState.State.AgentId);
        return $"Started task {taskId}: {ret}";
    }
    public ValueTask<AgentState> GetAgentInfo()
    {
        return GrainFactory.GetGrain<IAgent>(_userState.State.AgentId).AgentInfo();
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