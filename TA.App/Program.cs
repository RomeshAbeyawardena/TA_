using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Constants;
using TA.ServiceBroker;
using TA.Services;

namespace TA.App
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            await Task.WhenAll(DefaultAppBuilder<App>
                .CreateBuilder(entryPointAsync: app => app.Begin())
                .RegisterServices(RegisterServices)
                .RegisterServicesFromAssemblies<TAServiceBroker>()
                .StartAsync(), CreateWebHostBuilder(args)
                .Build()
                .RunAsync());
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost
            .CreateDefaultBuilder<Startup>(args)
            .ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile(General.DefaultJsonAppSetting))
            .UseKestrel(options => options.Listen(IPAddress.Any, 50000));

        public static void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>();
        }
    }
}