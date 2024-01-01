using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.RateLimiting;

namespace Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class RateLimitServiceCollectionExtensions
    {
        public static void AddRateLimitExtension(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy("fixed-by-ip", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 1000,
                            Window = TimeSpan.FromSeconds(30),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 100
                        }));
            });
        }
    }
}
