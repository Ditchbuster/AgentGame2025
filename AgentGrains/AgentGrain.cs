using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class AgentGrain : Grain, IAgent
{
    private readonly ILogger _logger;

    private readonly IPersistentState<AgentState> _AgentState;
    public AgentGrain(ILogger<AgentGrain> logger, [PersistentState("agentState")] IPersistentState<AgentState> agentState)
    {
        _logger = logger;
        _AgentState = agentState;
    }
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_AgentState.State.AgentId))
        {
            _AgentState.State.AgentId = this.GetPrimaryKeyString();
            _AgentState.State.Name = "TestAgent";
            _AgentState.State.Xp = 0;
            _AgentState.WriteStateAsync(cancellationToken);
        }
        return base.OnActivateAsync(cancellationToken);
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<AgentState> AgentInfo()
    {
        return ValueTask.FromResult(_AgentState.State);
    }

    public ValueTask<int> AddXp(int amount)
    {
        _AgentState.State.Xp += amount;
        _AgentState.WriteStateAsync();
        return ValueTask.FromResult(_AgentState.State.Xp);
    }

}