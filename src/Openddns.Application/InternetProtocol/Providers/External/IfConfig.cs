using System.Net.Http;
using System.Threading.Tasks;

namespace Openddns.Application.InternetProtocol.Providers.External
{
    internal class IfConfig : IAddressProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IfConfig(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExternalAddress()
        {
            var httpClient = _httpClientFactory.CreateClient("InternetProviderClient");
            var response = await httpClient.GetAsync("https://ifconfig.io/ip");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
