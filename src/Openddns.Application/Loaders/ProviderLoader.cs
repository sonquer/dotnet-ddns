using Openddns.Providers.Interfaces;
using Openddns.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Openddns.Application.Loaders
{
    public class ProviderLoader : IProviderPluginLoader
    {
        private readonly Dictionary<string, IProvider> _loadedProviders = new();

        public ProviderLoader(IServiceProvider serviceProvider)
        {
            var typesToActivate = typeof(IProvider)
                .Assembly
                .GetTypes()
                .Where(e => e.IsClass && e.IsAbstract == false && e.GetInterfaces().Contains(typeof(IProvider)));

            foreach (var typeToActivate in typesToActivate)
            {
                var constructor = typeToActivate.GetConstructors(BindingFlags.Instance | BindingFlags.Public).First();

                var serviceInstances = constructor.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .Select(serviceProvider.GetService)
                    .ToList();

                var instance = (IProvider)Activator.CreateInstance(typeToActivate, serviceInstances.ToArray())!;

                _loadedProviders.Add(instance.Name, instance);
            }
        }

        public IEnumerable<string> GetProviders() => _loadedProviders.Keys;

        public async Task Setup(IReadOnlyList<ProviderOptionsModel>? options, string globalInternetProtocolAddress)
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

                await _loadedProviders[option.Provider].Setup(option, globalInternetProtocolAddress);
            }
        }
    }
}