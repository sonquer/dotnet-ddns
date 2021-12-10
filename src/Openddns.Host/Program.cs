using DotNET.DDNS.Application.Loaders;
using DotNET.DDNS.Worker;
using Microsoft.AspNetCore;

var host = WebHost.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, configuration) =>
    {
        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true)
            .AddCommandLine(args)
            .AddEnvironmentVariables("DOTNET_DDNS_")
            .AddProviderConfigurations();
    })
    .UseStartup<Startup>()
    .Build();

await host.RunAsync();