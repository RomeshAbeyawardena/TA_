using Microsoft.Extensions.DependencyInjection;

namespace TA.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}