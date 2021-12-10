using System.Net.Http;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol.Providers.External
{
    internal class Custom : IAddressProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _customAddress;

        public Custom(IHttpClientFactory httpClientFactory, string customAddress)
        {
            _httpClientFactory = httpClientFactory;
            _customAddress = customAddress;
        }

        public async Task<string> GetExternalAddress()
        {
            var httpClient = _httpClientFactory.CreateClient("InternetProviderClient");
            var response = await httpClient.GetAsync(_customAddress);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
