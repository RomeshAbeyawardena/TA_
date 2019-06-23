using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TA.Domains.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TService GetRequiredService<TService>(this IServiceCollection services)
        {
            return services.BuildServiceProvider()
                .GetRequiredService<TService>();
        }

        public static IServiceCollection RegisterServicesFromAssemblies<TServiceRegistration, TServiceBroker>(
            this IServiceCollection services, Func<TServiceBroker, IEnumerable<Assembly>> getAssemblies,
            Action<TServiceRegistration, IServiceCollection> registerServices) 
            where TServiceRegistration : class
        {
            foreach (var a in getAssemblies(Activator.CreateInstance<TServiceBroker>()))
            {
                foreach (var t in a.GetTypes().Where(type => type.IsClass && type.GetInterface("IServiceRegistration") == typeof(TServiceRegistration)))
                {
                    var serviceRegistration = Activator.CreateInstance(t) as TServiceRegistration;
                    registerServices(serviceRegistration, services);
                }
            }

            return services;
        }
    }
}