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

   using IHost host = builder.Build();

if (AnsiConsole.Confirm("Connect?"))
{ //if yes then continue
    string choice = Menu();
    while (choice != "Exit")
    {
        switch (choice)
        {
            case "Connect":
                await host.StartAsync();
                IClusterClient client = host.Services.GetRequiredService<IClusterClient>();
                IHello friend = client.GetGrain<IHello>(0);
                string response = await friend.SayHello("Hi friend!");
                AnsiConsole.WriteLine($"{response}");
                break;
            case "Generate World":
                AnsiConsole.WriteLine("Not implemented");
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

static string GenerateWorld()
{
    return "Not implemented";
}

static string Menu()
{
    Rule rule = new Rule("[red]Menu[/]");
    rule.Justification = Justify.Left;
    AnsiConsole.Write(rule);
    string sel = AnsiConsole.Prompt(new SelectionPrompt<string>().PageSize(20).AddChoices(["Connect", "Generate World", "Exit"]));
    return sel;
}
