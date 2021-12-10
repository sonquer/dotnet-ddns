using DotNET.DDNS.Providers.Models;
using System.Threading.Tasks;

namespace DotNET.DDNS.Providers
{
    public interface IProvider
    {
        string Name { get; }

        public Task Setup(ProviderOptionsModel providerOptionsModel, string globalInternetProtocolAddress);
    }
}