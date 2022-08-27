using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Openddns.Core.Interfaces;
using Openddns.Infrastructure.SQLite.Repositories;
using System.Runtime.InteropServices;

namespace Openddns.Infrastructure.SQLite.Installers
{
    public class InfrastructureLayerInstaller : IInstaller
    {
        private readonly IConfiguration _configuration;

        public InfrastructureLayerInstaller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<DatabaseContext>(contextOptionsBuilder =>
            {
                var databasePath = "./database.db";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    databasePath = "/data/database.db";
                }

                if (string.IsNullOrWhiteSpace(_configuration["DATABASE_PATH"]) == false)
                {
                    databasePath = _configuration["DATABASE_PATH"];
                }

                contextOptionsBuilder.UseSqlite(@$"Data Source={databasePath};");
            });

            serviceCollection.AddScoped<IRepository, Repository>();

            return serviceCollection;
        }
    }
}
