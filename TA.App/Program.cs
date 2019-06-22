using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.ServiceBroker;
using TA.Services;

namespace TA
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            await Task.WhenAll(DefaultAppBuilder<App>
                .CreateBuilder(entryPointAsync: app => app.Begin())
                .RegisterServices(RegisterServices)
                .RegisterServicesFromAssemblies<TAServiceBroker>()
                .StartAsync(), CreateWebHostBuilder(args).Build().StartAsync());
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder<Startup>(args);

        public static void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>();
        }
    }
}