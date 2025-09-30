using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class AgentGrain : Grain, IAgent
{
    private readonly ILogger _logger;

    private readonly IPersistentState<AgentState> _agentState;
    private readonly IPersistentState<InventoryState> _inventoryState;
    public AgentGrain(ILogger<AgentGrain> logger, [PersistentState("agentState")] IPersistentState<AgentState> agentState, [PersistentState("inventoryState")] IPersistentState<InventoryState> inventoryState)
    {
        _logger = logger;
        _agentState = agentState;
        _inventoryState = inventoryState;
    }
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_agentState.State.AgentId))
        {
            _agentState.State.AgentId = this.GetPrimaryKeyString();
            _agentState.State.Name = "TestAgent";
            _agentState.State.Xp = 0;
            _agentState.WriteStateAsync(cancellationToken);
        }
        return base.OnActivateAsync(cancellationToken);
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<AgentState> AgentInfo()
    {
        return ValueTask.FromResult(_agentState.State);
    }

    public ValueTask<int> AddXp(int amount)
    {
        _agentState.State.Xp += amount;
        _agentState.WriteStateAsync();
        return ValueTask.FromResult(_agentState.State.Xp);
    }

    public ValueTask<int> AddItem(string itemId, int amount)
    {
        if (_inventoryState.State.Items.ContainsKey(itemId))
        {
            _inventoryState.State.Items[itemId] += amount;
        }
        else
        {
            _inventoryState.State.Items[itemId] = amount;
        }
        _inventoryState.WriteStateAsync();
        return ValueTask.FromResult(_inventoryState.State.Items[itemId]);
    }
}