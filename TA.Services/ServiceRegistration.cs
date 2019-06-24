﻿using System;
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
                .AddSingleton<ICryptographyProvider, CryptographyProvider>()
                .AddScoped<ITokenKeyGenerator, TokenKeyGenerator>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddScoped<ISiteService, SiteService>()
                .AddScoped<IAssetService, AssetService>()
                .AddScoped<ITokenService, TokenService>()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}