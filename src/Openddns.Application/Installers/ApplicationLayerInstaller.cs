using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Openddns.Application.InternetProtocol;
using Openddns.Application.Loaders;
using Openddns.Core.Interfaces;
using System;

namespace Openddns.Application.Installers
{
    public class ApplicationLayerInstaller : IInstaller
    {
        public IServiceCollection Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IProviderPluginLoader, ProviderLoader>()
                .AddSingleton<IExternalInternetProtocolAddressRecognizer, ExternalInternetProtocolAddressRecognizer>();

            serviceCollection.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return serviceCollection;
        }
    }
}
