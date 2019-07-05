using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Constants;
using TA.ServiceBroker;
using TA.Services;

namespace TA.App
{
    public static class Program
    {
        [DllImport("User32.dll", CharSet=CharSet.Unicode)] 
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        public static async Task<int> Main(string[] args)
        {
            var m = MessageBox((IntPtr)0, "your mum", "My Message Box", 1);
            await Task.WhenAll(DefaultAppBuilder<App>
                .CreateBuilder(entryPointAsync: app => app.Begin())
                .RegisterServices(RegisterServices)
                .RegisterServicesFromAssemblies<TAServiceBroker>()
                .StartAsync(), CreateWebHostBuilder(args)
                .Build()
                .RunAsync());
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost
            .CreateDefaultBuilder<Startup>(args)
            .ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile(General.DefaultJsonAppSetting))
            .UseKestrel(options => options.Listen(IPAddress.Any, 50000));

        public static void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApplicationSettings, TAApplicationSettings>();
        }
    }
}