using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Contracts.Services;
using TA.Domains.Models;
using WebToolkit.Contracts;
using WebToolkit.Common.Extensions;
using Permission = TA.Domains.Models.Permission;

namespace TA.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                //.AddHostedService<CacheFlusherHostedService>()
                .RegisterProviders()
                .AddSingleton<INotificationHandler, DefaultNotificationHandler>()
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