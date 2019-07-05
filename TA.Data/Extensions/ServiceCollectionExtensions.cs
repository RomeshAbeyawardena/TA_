using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using WebToolkit.Contracts.Data;

namespace TA.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories<TDbContext>(this IServiceCollection services, params Type[] entities) where TDbContext : DbContext
        {
            foreach (var entity in entities)
            {
                var repositoryDefinitionType = typeof(IRepository<>);
                var repositoryType = typeof(DefaultRepository<,>);

                var genericRepositoryDefinitionType = repositoryDefinitionType.MakeGenericType(entity);
                var genericRepositoryType = repositoryType.MakeGenericType(typeof(TDbContext), entity);

                services.AddScoped(genericRepositoryDefinitionType, genericRepositoryType);
            }

            return services;
        }
    }
}