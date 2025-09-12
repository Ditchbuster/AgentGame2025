namespace GrainInterfaces;

public interface ITask : IGrainWithStringKey
{
    ValueTask<string> DebugDump();
    ValueTask<bool> StartTask(string agentId);
    ValueTask<string> Status();
}