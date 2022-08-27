using Microsoft.Extensions.DependencyInjection;

namespace Openddns.Core.Interfaces
{
    public interface IInstaller
    {
        IServiceCollection Install(IServiceCollection serviceCollection);
    }
}
