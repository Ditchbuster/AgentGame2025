using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Timers;

namespace AgentGrains;

public class LocationGrain : Grain, ILocation
{
    private readonly ILogger _logger;

    private readonly IPersistentState<LocationState> _locationState;
    private readonly IPersistentState<InventoryState> _inventoryState;
    public LocationGrain(ILogger<LocationGrain> logger, [PersistentState("locationState")] IPersistentState<LocationState> locationState, [PersistentState("inventoryState")] IPersistentState<InventoryState> inventoryState)
    {
        _logger = logger;
        _locationState = locationState;
        _inventoryState = inventoryState;
    }
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_locationState.State.Name))
        {
            _locationState.State.Name = "TestLocation";
            _locationState.State.Type = LocationType.Settlement;
            _locationState.WriteStateAsync(cancellationToken);
            _inventoryState.State.Items["wood"] = 10;
            _inventoryState.WriteStateAsync(cancellationToken);
        }
        this.RegisterGrainTimer(
            callback: UpdateState,
            options: new GrainTimerCreationOptions
            {
                DueTime = TimeSpan.Zero,
                KeepAlive = true,
                Period = TimeSpan.FromSeconds(10)
            });

        return base.OnActivateAsync(cancellationToken);
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<LocationState> LocationInfo()
    {
        return ValueTask.FromResult(_locationState.State);
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
    public ValueTask<List<string>> AvailibleTasks()
    {
        return ValueTask.FromResult(new List<string> { "gatherWood", "buildStructure" });
    }
    private Task UpdateState()
    {
        //Periodic updates here
        _inventoryState.State.Items["wood"] -= 1;
        if (_inventoryState.State.Items["wood"] < 0)
        {
            _inventoryState.State.Items["wood"] = 0;
            _locationState.State.Presence -= 1;
            if (_locationState.State.Presence < 0) _locationState.State.Presence = 0;
            _logger.LogInformation($"Location {_locationState.State.Name} lost presence due to lack of resources");
            _locationState.WriteStateAsync();
            _inventoryState.WriteStateAsync();
        }
        else
        {
            _locationState.State.Presence += 1;
            _locationState.WriteStateAsync();
            _logger.LogInformation($"Location {_locationState.State.Name} gained presence {_locationState.State.Presence}");
        }
        return Task.CompletedTask;
    }
}