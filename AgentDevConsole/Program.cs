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

    AnsiConsole.Confirm("Exiting...");

    await host.StopAsync();

}

static string Menu()
{
    Rule rule = new Rule("[red]Menu[/]");
    rule.Justification = Justify.Left;
    AnsiConsole.Write(rule);
    string sel = AnsiConsole.Prompt(new SelectionPrompt<string>().PageSize(20).AddChoices(new[] { "Task List", "Task Status", "Agent Info", "Exit" }));
    return sel;   
}