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
            _locationState.WriteStateAsync(cancellationToken);
        }
        this.RegisterGrainTimer(
            callback: _ =>
            {
                _logger.LogInformation("LocationGrain Timer Triggered");
                return Task.CompletedTask;
            },
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
}