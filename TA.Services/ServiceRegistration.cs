using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;

namespace TA.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                .AddScoped<ISiteService, SiteService>()
                .AddScoped<IAssetService, AssetService>();
        }
    }
}