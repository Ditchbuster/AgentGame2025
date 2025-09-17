namespace GrainInterfaces;

public interface IAgent : IGrainWithStringKey
{
    ValueTask<string> DebugDump();
    ValueTask<Agent> AgentInfo();
    ValueTask<int> AddXp(int amount);
}