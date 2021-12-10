using DotNET.DDNS.Application.InternetProtocol.Factories;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol
{
    public class ExternalInternetProtocolAddressRecognizer : IExternalInternetProtocolAddressRecognizer
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalInternetProtocolAddressRecognizer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExternalAddress(IConfiguration configuration)
        {
            var internetProtocolFactory = new InternetProtocolProviderFactory(_httpClientFactory);
            return await internetProtocolFactory.GetExternalAddress(configuration);
        }
    }
}