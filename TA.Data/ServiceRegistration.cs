using Microsoft.Extensions.DependencyInjection;

namespace TA.Data
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<TADbContext>();
        }
    }
}