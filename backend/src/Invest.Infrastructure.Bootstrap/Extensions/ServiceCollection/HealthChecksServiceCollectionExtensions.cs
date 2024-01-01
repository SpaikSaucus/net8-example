using Microsoft.Extensions.DependencyInjection;

namespace Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class HealthChecksServiceCollectionExtensions
    {
        public static void AddHealthChecksExtension(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }
    }
}
