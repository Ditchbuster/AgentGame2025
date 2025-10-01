using Microsoft.VisualBasic;

namespace GrainInterfaces;

[GenerateSerializer]
[Alias("GrainInterfaces.AgentState")]
public record class AgentState
{
    [Id(0)]
    public string AgentId { get; set; } = string.Empty;
    [Id(1)]
    public string Name { get; set; } = string.Empty;
    [Id(2)]
    public int Xp { get; set; } = 0;
}
[GenerateSerializer]
[Alias("GrainInterfaces.UserState")]
public record class UserState
{
    [Id(0)]
    public string AgentId { get; set; } = string.Empty;
}
[GenerateSerializer]
[Alias("GrainInterfaces.InventoryState")]
public record class InventoryState
{
    [Id(0)]
    public Dictionary<string, int> Items { get; set; } = [];
}

[GenerateSerializer]
[Alias("GrainInterfaces.LocationState")]
public record class LocationState
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;
    [Id(1)]
    public LocationType Type { get; set; } = LocationType.Ocean;
    [Id(2)]
    public int Presence { get; set; } = 0;
}

public enum LocationType
    {
        Settlement,
        Ocean,
        Wilderness
    }
public enum TaskType
{
    Gather,
    Build,
    Explore,
    Trade,
    Combat
}

[GenerateSerializer]
public record class TaskRecord
{
    [Id(0)]
    public string TaskId { get; set; } = string.Empty;
    [Id(1)]
    public string Name { get; set; } = string.Empty;
    [Id(2)]
    public TaskType Type { get; set; } = TaskType.Gather;
    [Id(3)]
    public int DurationMinutes { get; set; } = 0;
    [Id(4)]
    public Dictionary<string, int> Rewards { get; set; } = [];
}