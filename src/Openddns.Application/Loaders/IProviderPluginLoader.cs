using System.Collections.Generic;
using System.Threading.Tasks;
using Openddns.Providers.Models;

namespace Openddns.Application.Loaders
{
    public interface IProviderPluginLoader
    {
        IEnumerable<string> GetProviders();

        Task Setup(IReadOnlyList<ProviderOptionsModel>? options, string globalInternetProtocolAddress);
    }
}
