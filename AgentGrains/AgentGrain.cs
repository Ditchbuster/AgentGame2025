using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class AgentGrain : Grain, IAgent
{
    private readonly ILogger _logger;

    private Agent _agent;
    private int _xp;
    public AgentGrain(ILogger<AgentGrain> logger)
    {
        _logger = logger;
        _agent = new("test", "Test Name", 0);
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<Agent> AgentInfo()
    {
        return ValueTask.FromResult(_agent);
    }

    public ValueTask<int> AddXp(int amount)
    {
        _xp += amount;
        return ValueTask.FromResult(_agent.xp);
    }

}