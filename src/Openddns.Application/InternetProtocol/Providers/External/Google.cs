using System.Net.Http;
using System.Threading.Tasks;

namespace Openddns.Application.InternetProtocol.Providers.External
{
    internal class Google : IAddressProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Google(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExternalAddress()
        {
            var httpClient = _httpClientFactory.CreateClient("InternetProviderClient");
            var response = await httpClient.GetAsync("https://domains.google.com/checkip");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
