using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Contracts.Providers;
using TA.Contracts.Services;
using TA.Domains.Models;
using TA.Services.Extensions;
using TA.Services.Providers;
using WebToolkit.Common.Providers;
using WebToolkit.Contracts.Providers;
using Permission = TA.Domains.Models.Permission;

namespace TA.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddHostedService<DefaultNotificationHandler>()
                .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                .AddSingleton<IMapperProvider, MapperProvider>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>()
                .AddSingleton<ICacheProvider, CacheProvider>()
                .AddSingleton<IAsyncLockDictionary, DefaultAsyncLockDictionary>()
                .AddDefaultValueProvider<Site>(site => { site.IsActive = true; })
                .AddDefaultValueProvider<Asset>(asset => { asset.IsActive = true; })
                .AddDefaultValueProvider<Token>(token => { token.IsActive = true; })
                .AddDefaultValueProvider<Permission>()
                .AddScoped<ITokenKeyGenerator, TokenKeyGenerator>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddScoped<ISiteService, SiteService>()
                .AddScoped<IAssetService, AssetService>()
                .AddScoped<ITokenService, TokenService>()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}