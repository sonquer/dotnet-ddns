using DotNET.DDNS.Providers;
using DotNET.DDNS.Providers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.Loaders
{
    public class ProviderPluginLoader : IProviderPluginLoader
    {
        private readonly Dictionary<string, IProvider> _loadedProviders = new();

        public ProviderPluginLoader(IServiceProvider serviceProvider)
        {
            var filesLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var providers = Directory.GetFiles(filesLocation, "*.Providers.*.dll");
            foreach (var provider in providers)
            {
                var typeToActivate = Assembly.LoadFile(provider).GetTypes()
                    .First(e => e.IsClass && e.IsAbstract == false && e.GetInterfaces().Contains(typeof(IProvider)));

                var constructor = typeToActivate.GetConstructors(BindingFlags.Instance | BindingFlags.Public).First();

                var serviceInstances = constructor.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .Select(serviceProvider.GetService)
                    .ToList();

                var instance = (IProvider) Activator.CreateInstance(typeToActivate, serviceInstances.ToArray());

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