using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;
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
                .AddSingleton<ISystemClock, SystemClock>()
                .RegisterServicesFromAssemblies<IServiceRegistration, TAServiceBroker>(
                    serviceBroker => serviceBroker.GetServiceAssemblies(),
                    (serviceRegistration, s) => serviceRegistration.RegisterServices(s))
                .AddDistributedMemoryCache(options => options.SizeLimit = 64000000)
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            return Do(services.BuildServiceProvider());
        }

        public IServiceProvider Do(ServiceProvider services)
        {
           var notificationHandler = services.GetRequiredService<INotificationHandler>();
           var aa = new System.Timers.Timer(1000);

            aa.Elapsed += Aa_Elapsed;
            aa.Start();
            
           return services;
        }

        private void Aa_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime.Ticks);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}