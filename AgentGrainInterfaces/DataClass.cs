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