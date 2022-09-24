using Openddns.Application.Cleaners;
using Openddns.Application.InternetProtocol;
using Openddns.Application.Loaders;
using Openddns.Core.Models;
using Openddns.Providers.Models;

namespace Openddns.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IConfiguration _configuration;

        private readonly IProviderPluginLoader _providerPluginLoader;

        private readonly IExternalInternetProtocolAddressRecognizer _externalInternetProtocolAddressRecognizer;

        private readonly ICleaner<LogModel> _logModelCleaner;

        public Worker(ILogger<Worker> logger, 
            IConfiguration configuration,
            IProviderPluginLoader providerPluginLoader, 
            IExternalInternetProtocolAddressRecognizer externalInternetProtocolAddressRecognizer,
            ICleaner<LogModel> logModelCleaner)
        {
            _logger = logger;
            _configuration = configuration;
            _providerPluginLoader = providerPluginLoader;
            _externalInternetProtocolAddressRecognizer = externalInternetProtocolAddressRecognizer;
            _logModelCleaner = logModelCleaner;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (int.TryParse(_configuration["PERIOD"], out var period) == false)
            {
                period = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            }

            var providerOptions = _configuration.GetSection("Providers").Get<IReadOnlyList<ProviderOptionsModel>>();
            if (providerOptions?.Count <= 0)
            {
                throw new InvalidOperationException("Provider options not found...");
            }

            foreach (var providerOption in providerOptions!)
            {
                _logger.LogInformation($"DNS provider '{providerOption.Provider}' found.");
            }

            while (stoppingToken.IsCancellationRequested == false)
            {
                await _logModelCleaner.Execute(stoppingToken);

                var globalInternetProtocolAddress = await _externalInternetProtocolAddressRecognizer.GetExternalAddress(_configuration);

                _logger.LogInformation($"Your IP = '{globalInternetProtocolAddress}'");

                await _providerPluginLoader.Setup(providerOptions, globalInternetProtocolAddress, stoppingToken);

                await Task.Delay(period, stoppingToken);
            }
        }
    }
}