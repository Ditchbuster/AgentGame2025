namespace GrainInterfaces;

public interface IAgent : IGrainWithStringKey
{
    ValueTask<string> DebugDump();
    ValueTask<AgentState> AgentInfo();
    ValueTask<int> AddXp(int amount);
    ValueTask<int> AddItem(string itemId, int amount);
}