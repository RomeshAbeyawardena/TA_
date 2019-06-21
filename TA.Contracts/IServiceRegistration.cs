using Microsoft.Extensions.DependencyInjection;

namespace TA
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}