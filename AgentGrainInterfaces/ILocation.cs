namespace GrainInterfaces;

public interface ILocation : IGrainWithStringKey
{
    ValueTask<string> DebugDump();
    ValueTask<LocationState> LocationInfo();
    ValueTask<int> AddItem(string itemId, int amount);
}