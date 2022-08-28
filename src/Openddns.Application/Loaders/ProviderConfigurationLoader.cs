using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Openddns.Application.Loaders
{
    public static class ProviderConfigurationLoader
    {
        public static IConfigurationBuilder AddProviderConfigurations(this IConfigurationBuilder configurationBuilder)
        {
            var paths = new List<string>
            {
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Providers")
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                paths.Add("/config");
            }

            var progress = 1;
            foreach (var path in paths)
            {
                Console.WriteLine($"[{progress++}/{paths.Count}] Loading additional configurations from '{path}'");
                Directory.CreateDirectory(path);

                var files = Directory.GetFiles(path, "*.json").ToList();
                files.ForEach(e => configurationBuilder.AddJsonFile(e, true));
            }

            return configurationBuilder;
        }
    }
}
