using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TA
{
    public interface IAppBuilder<out TStart> where TStart : class
    {
        IAppBuilder<TStart> RegisterServicesFromAssemblies<TServiceBroker>() where TServiceBroker : IServiceBroker;
        IAppBuilder<TStart> ConfigureEntryPoints(Func<TStart, int> entryPoint, Func<TStart, Task<int>> entryPointAsync = null);
        TService GetRequiredService<TService>();
        IServiceProvider ServiceProvider { get; }
        IServiceCollection Services { get; }
        IAppBuilder<TStart> RegisterServices(Action<IServiceCollection> serviceRegistration = null);
        int Start();
        Task<int> StartAsync();
    }
}