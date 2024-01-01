using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class ApiVersioningServiceCollectionExtensions
    {
        public static void AddApiVersionExtension(this IServiceCollection services, string defaultVersion)
        {

            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(double.Parse(defaultVersion));
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                            new UrlSegmentApiVersionReader(),
                            new QueryStringApiVersionReader("api-version"),
                            new HeaderApiVersionReader("X-Version"),
                            new MediaTypeApiVersionReader("x-version"));
                })
            .AddMvc(
                options =>
                {
                    options.Conventions.Add(new VersionByNamespaceConvention());
                })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
