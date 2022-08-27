using System.Threading.Tasks;
using Openddns.Providers.Models;

namespace Openddns.Providers
{
    public interface IProvider
    {
        string Name { get; }

        public Task Setup(ProviderOptionsModel providerOptionsModel, string globalInternetProtocolAddress);
    }
}