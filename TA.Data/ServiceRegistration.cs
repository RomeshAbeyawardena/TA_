﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Models;
using WebToolkit.Contracts;
using WebToolkit.Common.Extensions;
using Permission = TA.Domains.Models.Permission;

namespace TA.Data
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            var applicationSettings = services
                .GetRequiredService<IApplicationSettings>();
            
            services
                .AddDbContext<TADbContext>(options => options
                    .UseSqlServer(applicationSettings.ConnectionString)
#if DEBUG
                    .EnableSensitiveDataLogging())
#endif
                .RegisterRepositories<TADbContext>(
                    typeof(Asset), 
                    typeof(Permission), 
                    typeof(Site), 
                    typeof(Token), 
                    typeof(TokenPermission),
                    typeof(User));
        }
    }
}