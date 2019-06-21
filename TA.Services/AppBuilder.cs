using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TA.Services
{
    public class DefaultAppBuilder<TStart> : IAppBuilder<TStart> where TStart : class
    {
        private IServiceCollection _serviceCollection;
        private TStart Startup => ServiceProvider.GetRequiredService<TStart>();
        private Func<TStart, int> _entryPoint;
        private Func<TStart, Task<int>> _entryPointAsync;

        private IAppBuilder<TStart> RegisterServicesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (var a in assemblies)
            {
                foreach (var t in a.GetTypes().Where(type => type.IsClass && type.GetInterface("IServiceRegistration") == typeof(IServiceRegistration)))
                {
                    var serviceRegistration = Activator.CreateInstance(t) as IServiceRegistration;
                    RegisterServices((services) => serviceRegistration?.RegisterServices(services));
                }
            }

            return this;
        }

        private void Initialise()
        {
            Services
                .AddSingleton<IAppBuilder<TStart>>(this)
                .AddSingleton<TStart>();

        }

        public IAppBuilder<TStart> RegisterServicesFromAssemblies<TServiceBroker>() where TServiceBroker : IServiceBroker
        {
            return RegisterServicesFromAssemblies(Activator.CreateInstance<TServiceBroker>().GetServiceAssemblies());
        }

        public IAppBuilder<TStart> ConfigureEntryPoints(Func<TStart, int> entryPoint, Func<TStart, Task<int>> entryPointAsync = null)
        {
            _entryPoint = entryPoint;
            _entryPointAsync = entryPointAsync;

            return this;
        }

        public TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        public IServiceCollection Services => _serviceCollection ?? (_serviceCollection = new ServiceCollection());
        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        private DefaultAppBuilder(Func<TStart, int> entryPoint, Func<TStart, Task<int>> entryPointAsync = null)
        {
            _entryPoint = entryPoint;
            _entryPointAsync = entryPointAsync;
        }

        public static IAppBuilder<TStart> CreateBuilder(Func<TStart, int> entryPoint = null, Func<TStart, Task<int>> entryPointAsync = null)
        {
            return new DefaultAppBuilder<TStart>(entryPoint, entryPointAsync);
        }

        public IAppBuilder<TStart> RegisterServices(Action<IServiceCollection> serviceRegistration = null)
        {
            serviceRegistration?.Invoke(Services);
            
            return this;
        }

        public int Start()
        {
            if(_entryPoint == null && _entryPointAsync == null)
                throw new NotImplementedException();
            
            Initialise();
            return _entryPoint?.Invoke(Startup) ?? _entryPointAsync(Startup).Result;
        }

        public async Task<int> StartAsync()
        {
            if(_entryPointAsync == null)
                throw new NotImplementedException();

            Initialise();
            return await _entryPointAsync(Startup);
        }
    }
}