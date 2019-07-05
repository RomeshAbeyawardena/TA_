using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;
using TA.Contracts;
using TA.Domains.Extensions;
using TA.ServiceBroker;

namespace TA.App
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>()
                .AddSingleton<ISystemClock, SystemClock>()
                .RegisterServicesFromAssemblies<IServiceRegistration, TAServiceBroker>(
                    serviceBroker => serviceBroker.GetServiceAssemblies(),
                    (serviceRegistration, s) => serviceRegistration.RegisterServices(s))
                .AddDistributedMemoryCache(options => options.SizeLimit = 64000000)
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            return services.BuildServiceProvider();
        }

         
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}