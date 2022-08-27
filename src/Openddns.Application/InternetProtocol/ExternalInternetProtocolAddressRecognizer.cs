using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Openddns.Application.InternetProtocol.Factories;

namespace Openddns.Application.InternetProtocol
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