using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Extensions;
using TA.ServiceBroker;

namespace TA
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>()
                .RegisterServicesFromAssemblies<IServiceRegistration, TAServiceBroker>(
                    serviceBroker => serviceBroker.GetServiceAssemblies(),
                    (serviceRegistration, s) => serviceRegistration.RegisterServices(s))
                .AddMvc();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}