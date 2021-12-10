using DotNET.DDNS.Application.InternetProtocol;
using DotNET.DDNS.Application.Loaders;

namespace DotNET.DDNS.Worker
{
    public class Startup
    {
        /// <summary>
        /// Configuration interface
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Web hosting environment
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration">Configuration interface</param>
        /// <param name="webHostEnvironment">Hosting environment</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProviderPluginLoader, ProviderPluginLoader>()
                .AddSingleton<IExternalInternetProtocolAddressRecognizer, ExternalInternetProtocolAddressRecognizer>()
                .AddHttpClient("InternetProviderClient", client =>
                {
                    if (int.TryParse(Configuration["HTTP_TIMEOUT"], out var timeout) == false)
                    {
                        timeout = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;
                    }

                    client.Timeout = TimeSpan.FromSeconds(timeout);
                });

            services.AddControllersWithViews()
                .AddControllersAsServices();

            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
