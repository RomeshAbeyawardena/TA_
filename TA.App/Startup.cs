using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TA.Contracts;
using TA.Domains.Extensions;
using TA.ServiceBroker;
using WebToolkit.Common;
using WebToolkit.Contracts;

namespace TA.App
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serializer = JsonSerializer.CreateDefault();
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>()
                .AddSingleton<ISystemClock, SystemClock>()
                .RegisterServicesFromAssemblies<IServiceRegistration, TAServiceBroker>(
                    serviceBroker => serviceBroker.GetServiceAssemblies(),
                    (serviceRegistration, s) => serviceRegistration.RegisterServices(s))
                .AddDistributedMemoryCache(options => options.SizeLimit = 64000000)
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services.AddSingleton<IJSonSettings>(
                new DefaultJSonSettings(settings =>
                {
                    settings.LoadSettings = new JsonLoadSettings();
                    settings.Serializer = serializer;
                }))
                .BuildServiceProvider();
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}