using DotNET.DDNS.Application.InternetProtocol;
using DotNET.DDNS.Application.Loaders;
using DotNET.DDNS.Worker;
using System.Reflection;

IConfiguration? config = null;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, configuration) =>
    {
        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true)
            .AddCommandLine(args)
            .AddEnvironmentVariables("DOTNET_DDNS_");

        var filesLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        Directory.CreateDirectory(Path.Combine(filesLocation, "Providers"));

        var configurations = Directory.GetFiles(filesLocation, "Providers/*.json").ToList();
        configurations.ForEach(e => configuration.AddJsonFile(e, true));

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
