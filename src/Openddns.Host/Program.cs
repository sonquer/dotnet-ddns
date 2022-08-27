using Microsoft.AspNetCore;
using Openddns.Application.Loaders;
using Openddns.Host;

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