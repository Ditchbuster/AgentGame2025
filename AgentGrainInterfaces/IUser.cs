namespace GrainInterfaces;

public interface IUser : IGrainWithStringKey
{
    ValueTask<string> SayHello(string greeting);
    ValueTask<string> DebugDump();
    ValueTask<List<string>> AvailibleTasks();
}