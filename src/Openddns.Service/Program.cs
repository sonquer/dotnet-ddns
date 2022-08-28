using MediatR;
using Microsoft.EntityFrameworkCore;
using Openddns.Application.InternetProtocol;
using Openddns.Application.Loaders;
using Openddns.Core.Interfaces;
using Openddns.Infrastructure;
using Openddns.Infrastructure.Repositories;
using System.Runtime.InteropServices;

namespace Openddns.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureAppConfiguration((host, configuration) =>
            {
                configuration
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables("DOTNET_DDNS_")
                    .AddProviderConfigurations();
            });

            builder.Services.AddDbContext<DatabaseContext>(contextOptionsBuilder =>
            {
                var databasePath = "./database.db";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    databasePath = "/config/database.db";
                }

                if (string.IsNullOrWhiteSpace(builder.Configuration["DATABASE_PATH"]) == false)
                {
                    databasePath = builder.Configuration["DATABASE_PATH"];
                }

                var migrationsAssembly = typeof(Program).Namespace;
                contextOptionsBuilder.UseSqlite(@$"Data Source={databasePath};", b => b.MigrationsAssembly(migrationsAssembly));
            });

            builder.Services.AddScoped<IRepository, Repository>();

            builder.Services.AddSingleton<IProviderPluginLoader, ProviderLoader>()
                .AddSingleton<IExternalInternetProtocolAddressRecognizer, ExternalInternetProtocolAddressRecognizer>();

            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews();

            builder.Services.AddHostedService<Worker>();

            var app = builder.Build();
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            if (app.Environment.IsDevelopment() == false)
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}