using System.Net.Http;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol.Providers.External
{
    internal class OpenDNS : IAddressProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenDNS(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExternalAddress()
        {
            var httpClient = _httpClientFactory.CreateClient("InternetProviderClient");
            var response = await httpClient.GetAsync("https://diagnostic.opendns.com/myip");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
