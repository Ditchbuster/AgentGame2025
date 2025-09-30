using Spectre.Console;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GrainInterfaces;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

if (AnsiConsole.Confirm("Connect?"))
{ //if yes then continue
    string userId = AnsiConsole.Ask<string>("User id:", "0");
    AnsiConsole.WriteLine(userId);
    using IHost host = builder.Build();
    await host.StartAsync();

    IClusterClient client = host.Services.GetRequiredService<IClusterClient>();

    IHello friend = client.GetGrain<IHello>(0);
    string response = await friend.SayHello("Hi friend!");

    AnsiConsole.WriteLine($"{response}");

    //Start

    IUser user = client.GetGrain<IUser>(userId);
    AnsiConsole.WriteLine(await user.DebugDump());

    string choice = Menu();
    while (choice != "Exit")
    {
        switch (choice)
        {
            case "Task Selection":
                var tasks = await user.AvailibleTasks();
                Rule rule = new Rule("[green]Tasks[/]");
                rule.Justification = Justify.Left;
                AnsiConsole.Write(rule);
                string sel = AnsiConsole.Prompt(new SelectionPrompt<string>().PageSize(20).AddChoices(tasks).AddChoices(["Cancel"]));
                if (sel == "Cancel") break;
                string res = await user.StartTask(sel);
                AnsiConsole.WriteLine(res);
                break;
            case "Task Status":
                AnsiConsole.WriteLine("Not implemented");
                break;
            case "Agent Info":
                AgentState agent = await user.GetAgentInfo();
                AnsiConsole.WriteLine($"Agent ID: {agent.AgentId}, Name: {agent.Name}, xp: {agent.Xp}");
                break;
            case "Test Location":
                ILocation loc = client.GetGrain<ILocation>("TestLocationId");
                var locinfo = await loc.LocationInfo();
                AnsiConsole.WriteLine($"Name: {locinfo.Name}");
                break;
            default:
                AnsiConsole.WriteLine("Unknown choice");
                break;
        }
        choice = Menu();
    }
    AnsiConsole.Confirm("Exiting...");

    await host.StopAsync();

}

static string Menu()
{
    Rule rule = new Rule("[red]Menu[/]");
    rule.Justification = Justify.Left;
    AnsiConsole.Write(rule);
    string sel = AnsiConsole.Prompt(new SelectionPrompt<string>().PageSize(20).AddChoices(["Task Selection", "Task Status", "Agent Info","Test Location", "Exit"]));
    return sel;   
}