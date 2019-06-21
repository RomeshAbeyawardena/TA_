using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TA.ServiceBroker;

namespace TA
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            return await DefaultAppBuilder<App>
                .CreateBuilder(entryPointAsync: app => app.Begin())
                .RegisterServicesFromAssemblies<TAServiceBroker>()
                .StartAsync();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            
        }
    }
}