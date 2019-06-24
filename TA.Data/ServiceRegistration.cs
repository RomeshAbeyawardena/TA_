using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Extensions;
using TA.Domains.Models;

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
                    .EnableSensitiveDataLogging())
                .AddScoped<IRepository<Site>, DefaultRepository<TADbContext, Site>>()
                .AddScoped<IRepository<Asset>, DefaultRepository<TADbContext, Asset>>()
                .AddScoped<IRepository<Token>, DefaultRepository<TADbContext, Token>>()
                .AddScoped<IRepository<TokenPermission>, DefaultRepository<TADbContext, TokenPermission>>();
        }
    }
}