using Microsoft.Extensions.DependencyInjection;

namespace TA.Domains.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TService GetRequiredService<TService>(this IServiceCollection services)
        {
            return services.BuildServiceProvider()
                .GetRequiredService<TService>();
        }
    }
}