using DotNET.DDNS.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.Loaders
{
    public interface IProviderPluginLoader
    {
        IEnumerable<string> GetProviders();

        Task Setup(IReadOnlyList<ProviderOptionsModel>? options, string globalInternetProtocolAddress);
    }
}
