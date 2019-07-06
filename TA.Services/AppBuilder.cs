using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using TA.Contracts;
using TA.Domains.Constants;
using WebToolkit.Contracts;

namespace TA.Services
{
    public class DefaultAppBuilder<TStart> : IAppBuilder<TStart> where TStart : class
    {
        private IServiceCollection _serviceCollection;
        private TStart Startup => ServiceProvider.GetRequiredService<TStart>();
        private Func<TStart, int> _entryPoint;
        private Func<TStart, Task<int>> _entryPointAsync;
        private bool _isConfigurationAdded;

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

        private async Task<int> LogException(Exception exception)
        {
            var defaultForeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Error.WriteLineAsync($"An error occurred: {exception.Message} Stack Trace: {exception.StackTrace}");
            Console.ForegroundColor = defaultForeColour;
            return exception.HResult;
        }

        private void Initialise()
        {
            Services
                .AddSingleton<ISystemClock, SystemClock>()
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

        private DefaultAppBuilder(Func<TStart, int> entryPoint, Func<TStart, 
            Task<int>> entryPointAsync = null)
        {
            _entryPoint = entryPoint;
            _entryPointAsync = entryPointAsync;
        }

        public static IAppBuilder<TStart> CreateBuilder(Func<TStart, int> entryPoint = null, 
            Func<TStart, Task<int>> entryPointAsync = null)
        {
            return new DefaultAppBuilder<TStart>(entryPoint, entryPointAsync);
        }

        public IAppBuilder<TStart> RegisterServices(Action<IServiceCollection> serviceRegistration = null)
        {
            if (!_isConfigurationAdded)
            {
                Services.AddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile(General.DefaultJsonAppSetting).Build());
                _isConfigurationAdded = true;
            }

            serviceRegistration?.Invoke(Services);
            
            return this;
        }

        public int Start(bool throwOnError = true)
        {
            if(_entryPoint == null && _entryPointAsync == null)
                throw new NotImplementedException();

            try
            {
                Initialise();
                return _entryPoint?.Invoke(Startup) ?? _entryPointAsync(Startup).Result;
            }
            catch (Exception ex)
            {
                var result = LogException(ex).Result;

                if (throwOnError)
                    throw;

                return result;
            }
        }

        public async Task<int> StartAsync(bool throwOnError = true)
        {
            if(_entryPointAsync == null)
                throw new NotImplementedException();

            try
            {
                Initialise();
                return await _entryPointAsync(Startup);
            }
            catch (Exception ex)
            {
                var result = await LogException(ex);

                if (throwOnError)
                    throw;

                return result;
            }
        }
    }
}