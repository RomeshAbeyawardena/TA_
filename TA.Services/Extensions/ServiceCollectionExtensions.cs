using System;
using Microsoft.Extensions.DependencyInjection;
using WebToolkit.Common.Providers;
using WebToolkit.Contracts.Providers;

namespace TA.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultValueProvider<TModel>(this IServiceCollection services, Action<TModel> defaults)
        {
            return services.AddSingleton<IDefaultValueProvider<TModel>>(new DefaultValuesProvider<TModel>(defaults));
        }

        
        public static IServiceCollection AddDefaultValueProvider<TModel>(this IServiceCollection services)
        {
            return services.AddSingleton<IDefaultValueProvider<TModel>>(new DefaultValuesProvider<TModel>(model => {}));
        }
    }
}