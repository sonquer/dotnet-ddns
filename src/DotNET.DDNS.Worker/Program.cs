using DotNET.DDNS.Application.InternetProtocol;
using DotNET.DDNS.Application.Loaders;
using DotNET.DDNS.Worker;

IConfiguration? config = null;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, configuration) =>
    {
        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true)
            .AddCommandLine(args)
            .AddEnvironmentVariables("DOTNET_DDNS_")
            .AddProviderConfigurations();

        config = configuration.Build();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IProviderPluginLoader, ProviderPluginLoader>()
            .AddSingleton<IExternalInternetProtocolAddressRecognizer, ExternalInternetProtocolAddressRecognizer>()
            .AddHttpClient("InternetProviderClient", client =>
            {
                if (int.TryParse(config!["HTTP_TIMEOUT"], out var timeout) == false)
                {
                    timeout = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;
                }

                client.Timeout = TimeSpan.FromSeconds(timeout);
            });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
