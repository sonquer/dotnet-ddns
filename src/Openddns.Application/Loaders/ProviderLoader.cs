using Microsoft.Extensions.DependencyInjection;
using Openddns.Providers.Interfaces;
using Openddns.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Loaders
{
    public class ProviderLoader : IProviderPluginLoader
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Setup(IReadOnlyList<ProviderOptionsModel>? options, string globalInternetProtocolAddress, CancellationToken cancellationToken)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options), "Provider options not found");
            }

            foreach (var option in options)
            {
                if (string.IsNullOrWhiteSpace(option.Provider))
                {
                    throw new ArgumentNullException(nameof(option.Provider), "Provider name in options is null or empty");
                }

                var typeToActivate = typeof(IProvider)
                    .Assembly
                    .GetTypes()
                    .First(e => e.IsClass && e.IsAbstract == false && e.GetInterfaces().Contains(typeof(IProvider)) && e.Name == option.Provider);

                using var scope = _serviceProvider.CreateScope();

                var constructor = typeToActivate.GetConstructors(BindingFlags.Instance | BindingFlags.Public).First();

                var serviceInstances = constructor.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .Select(scope.ServiceProvider.GetService)
                    .ToList();

                var instance = (IProvider)Activator.CreateInstance(typeToActivate, serviceInstances.ToArray())!;
                await instance.Setup(option, globalInternetProtocolAddress, cancellationToken);
            }
        }
    }
}