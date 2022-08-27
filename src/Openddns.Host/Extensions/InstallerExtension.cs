using System.Reflection;
using Openddns.Core.Interfaces;

namespace Openddns.Host.Extensions
{
    public static class InstallerExtension
    {
        public static IServiceCollection InstallDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("InternetProviderClient", client =>
            {
                if (int.TryParse(configuration["HTTP_TIMEOUT"], out var timeout) == false)
                {
                    timeout = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;
                }

                client.Timeout = TimeSpan.FromSeconds(timeout);
            });

            var assemblies = GetListOfEntryAssemblyWithReferences();
            foreach (var assembly in assemblies)
            {
                var installers = assembly.GetTypes()
                    .Where(e => e.IsAssignableTo(typeof(IInstaller)) && e.IsAbstract == false)
                    .ToList();

                if (installers.Count <= 0)
                {
                    continue;
                }

                foreach (var installer in installers)
                {
                    var configurationInConstructor = false;

                    var constructors = installer.GetConstructors();
                    foreach (var constructor in constructors)
                    {
                        foreach (var parameter in constructor.GetParameters())
                        {
                            if (parameter.ParameterType == typeof(IConfiguration))
                            {
                                configurationInConstructor = true;
                            }
                        }
                    }

                    var installerInstance = configurationInConstructor 
                        ? (IInstaller)Activator.CreateInstance(installer, configuration)!
                        : (IInstaller)Activator.CreateInstance(installer)!;

                    installerInstance.Install(services);
                }
            }

            services.AddControllersWithViews()
                .AddControllersAsServices();

            services.AddHostedService<Worker>();

            return services;
        }

        private static List<Assembly> GetListOfEntryAssemblyWithReferences()
        {
            var listOfAssemblies = new List<Assembly>
            {
                Assembly.GetEntryAssembly()!
            };

            listOfAssemblies.AddRange(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load));
            return listOfAssemblies;
        }
    }
}
