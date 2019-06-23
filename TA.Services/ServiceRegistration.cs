using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Services.Providers;
using WebToolkit.Common.Providers;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                .AddSingleton<IMapperProvider, MapperProvider>()
                .AddScoped<ISiteService, SiteService>()
                .AddScoped<IAssetService, AssetService>()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}