using Microsoft.Extensions.DependencyInjection;

namespace TA.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ILocationService, LocationService>();
        }
    }
}