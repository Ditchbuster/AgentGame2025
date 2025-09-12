namespace GrainInterfaces;

[GenerateSerializer, Immutable]
public record class Agent(
    string Id,
    string Name,
    int xp);