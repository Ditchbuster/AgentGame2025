using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace AgentGrains;

public class TaskGrain : Grain, ITask, IRemindable
{
    private readonly ILogger _logger;

    private string _agentId;
    private string _locationId;
    private bool _running;
    private int _reminderPulses;

    public TaskGrain(ILogger<TaskGrain> logger)
    {
        _logger = logger;
        _running = false;
        _reminderPulses = 0;
        _agentId = string.Empty;
        _locationId = string.Empty;
    }
    public ValueTask<string> DebugDump()
    {
        throw new NotImplementedException();
    }

    public ValueTask<List<string>> getAvailibleTasks() // TODO move to location grain
    {
        return ValueTask.FromResult(new List<string> { "testTaskId" });
    }

    public async ValueTask<bool> StartTask(string agentId) //TODO: update to accept agent record
    {
        _logger.LogInformation("Starting task for agent {AgentId}", agentId);
        if (!_running)
        {
            await this.RegisterOrUpdateReminder(
            reminderName: this.GetPrimaryKeyString(),
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromMinutes(1));
            _agentId = agentId;
            _running = true;
            _reminderPulses = 2;
        }
        return true;

    }

    public ValueTask<string> Status()
    {
        throw new NotImplementedException();
    }
    async Task IRemindable.ReceiveReminder(string reminderName, TickStatus status)
    {
        _reminderPulses--;
        Console.WriteLine($"Thanks for reminding me-- I almost forgot! {_reminderPulses}");
        if (_reminderPulses <= 0)
        {
            _logger.LogInformation("Task complete, unregistering reminder");
            var reminder =  await this.GetReminder(this.GetPrimaryKeyString());
            if (reminder != null)
            {
                await this.UnregisterReminder(reminder);
                IAgent agent = GrainFactory.GetGrain<IAgent>(this._agentId);
                ILocation location = GrainFactory.GetGrain<ILocation>(this._locationId);
                await agent.AddXp(10);
                await agent.AddItem("credits", 10);
                await location.AddItem("wood", 100);
                _logger.LogInformation("unregistered reminder");
            } else
            {
                _logger.LogWarning($"reminder {this.GetPrimaryKey()} was null");
            }
            _running = false;
        }
        return;
    }
}