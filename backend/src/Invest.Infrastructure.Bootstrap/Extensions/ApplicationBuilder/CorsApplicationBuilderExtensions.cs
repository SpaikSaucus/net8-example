using Microsoft.AspNetCore.Builder;

namespace Invest.Infrastructure.Bootstrap.Extensions.ApplicationBuilder
{
    public static class CorsApplicationBuilderExtensions
    {
        public static void UseCorsExtension(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}
