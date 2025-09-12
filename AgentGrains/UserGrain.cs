using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Grains;

public class UserGrain : Grain, IUser
{
    private readonly ILogger _logger;

    private Agent _agent;

    public UserGrain(ILogger<HelloGrain> logger)
    {
        _logger = logger;
        _agent = new("test", "Test Name", 0);
    }
    public ValueTask<string> DebugDump()
    {
        return ValueTask.FromResult(this.GetPrimaryKey().ToString());
    }

    public ValueTask<List<string>> AvailibleTasks()
    {
        return ValueTask.FromResult(new List<string> {"testTaskId"});
    }

    ValueTask<string> IUser.SayHello(string greeting)
    {
        _logger.LogInformation("""
            SayHello message received: greeting = "{Greeting}"
            """,
            greeting);
        
        return ValueTask.FromResult($"""

            Client said: "{greeting}", so HelloGrain says: Hello!
            """);
    }
}