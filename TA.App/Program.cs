using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.ServiceBroker;
using TA.Services;

namespace TA
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            return await DefaultAppBuilder<App>
                .CreateBuilder(entryPointAsync: app => app.Begin())
                .RegisterServices(RegisterServices)
                .RegisterServicesFromAssemblies<TAServiceBroker>()
                .StartAsync();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>();
        }
    }
}