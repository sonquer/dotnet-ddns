using Openddns.Application.InternetProtocol;
using Openddns.Application.Loaders;
using Openddns.Providers.Models;

namespace Openddns.Host
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IConfiguration _configuration;

        private readonly IProviderPluginLoader _providerPluginLoader;

        private readonly IExternalInternetProtocolAddressRecognizer _externalInternetProtocolAddressRecognizer;

        public Worker(ILogger<Worker> logger, 
            IConfiguration configuration,
            IProviderPluginLoader providerPluginLoader, 
            IExternalInternetProtocolAddressRecognizer externalInternetProtocolAddressRecognizer)
        {
            _logger = logger;
            _configuration = configuration;
            _providerPluginLoader = providerPluginLoader;
            _externalInternetProtocolAddressRecognizer = externalInternetProtocolAddressRecognizer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (int.TryParse(_configuration["PERIOD"], out var period) == false)
            {
                period = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            }

            var providers = _providerPluginLoader.GetProviders();
            foreach (var provider in providers)
            {
                _logger.LogInformation($"DNS provider '{provider}' plugin is loaded.");
            }

            var providerOptions = _configuration.GetSection("Providers").Get<IReadOnlyList<ProviderOptionsModel>>();
            if (providerOptions?.Count <= 0)
            {
                throw new InvalidOperationException("Provider options not found...");
            }

            while (stoppingToken.IsCancellationRequested == false)
            {
                var globalInternetProtocolAddress = await _externalInternetProtocolAddressRecognizer.GetExternalAddress(_configuration);

                _logger.LogInformation($"Your IP = '{globalInternetProtocolAddress}'");

                await _providerPluginLoader.Setup(providerOptions, globalInternetProtocolAddress);

                await Task.Delay(period, stoppingToken);
            }
        }
    }
}