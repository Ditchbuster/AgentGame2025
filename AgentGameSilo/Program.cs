using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering()
        .AddMemoryGrainStorageAsDefault()
        .UseInMemoryReminderService();
    })
    .RunConsoleAsync();