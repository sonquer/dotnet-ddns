using DotNET.DDNS.Application.InternetProtocol.Enums;
using DotNET.DDNS.Application.InternetProtocol.Providers;
using DotNET.DDNS.Application.InternetProtocol.Providers.External;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol.Factories
{
    internal class InternetProtocolProviderFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InternetProtocolProviderFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExternalAddress(IConfiguration configuration)
        {
            if (Enum.TryParse(typeof(InternetProtocolProvider), configuration["PUBLIC_IP_PROVIDER"], out var internetProtocolProvider) == false)
            {
                internetProtocolProvider = InternetProtocolProvider.Google;
            }

            IAddressProvider addressProvider = internetProtocolProvider switch
            {
                InternetProtocolProvider.Custom => new Custom(_httpClientFactory, configuration["PUBLIC_IP_PROVIDER_ADDRESS"]),
                InternetProtocolProvider.Google => new Google(_httpClientFactory),
                InternetProtocolProvider.OpenDNS => new OpenDNS(_httpClientFactory),
                InternetProtocolProvider.IfConfig => new IfConfig(_httpClientFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(internetProtocolProvider), internetProtocolProvider, "Unknown external address provider")
            };

            return await addressProvider.GetExternalAddress();
        }
    }
}