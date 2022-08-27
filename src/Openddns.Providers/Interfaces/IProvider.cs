using Openddns.Providers.Models;
using System.Threading.Tasks;

namespace Openddns.Providers.Interfaces
{
    public interface IProvider
    {
        string Name { get; }

        public Task Setup(ProviderOptionsModel providerOptionsModel, string globalInternetProtocolAddress);
    }
}