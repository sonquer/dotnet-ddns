using Openddns.Providers.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Loaders
{
    public interface IProviderPluginLoader
    {
        Task Setup(IReadOnlyList<ProviderOptionsModel>? options, string globalInternetProtocolAddress, CancellationToken cancellationToken);
    }
}
