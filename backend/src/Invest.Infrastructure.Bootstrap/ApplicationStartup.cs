using Asp.Versioning.ApiExplorer;
using Autofac;
using CoreLibrary.ORM.Dapper;
using CoreLibrary.ORM.EF;
using CoreLibrary.UnitOfWork;
using Invest.Application.Behaviors;
using Invest.Infrastructure.Bootstrap.Extensions.ApplicationBuilder;
using Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection;
using Invest.Infrastructure.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.EntityFrameworkCore.Extensions;
using System.Text.Json;

namespace Invest.Infrastructure.Bootstrap
{
    public class ApplicationStartup
    {
        private readonly IConfiguration configuration;
        protected const string JwtPolicy = "JwtPolicy";        

        public ApplicationStartup(IConfiguration configuration)
        {
            this.configuration = configuration;            
        }

        public virtual void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                });
            });
            services.AddAuthenticationExtension(this.configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddAutofacExtension(this.configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsExtension();
            services.AddRateLimitExtension();
            services.AddApiVersionExtension(this.configuration.GetValue<string>("AppSettings:DefaultApiVersion", "1.0"));
            services.AddHealthChecksExtension();
            services.AddSwaggerGenExtension();
            services.AddResponseCompressionExtension();
            services.AddHttpClientFactoryExtension(this.configuration);
            services.AddHttpContextAccessor();
           
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ValidatorBehavior<,>).Assembly));
            services.AddControllers(o =>
                {
                    o.Filters.Add(new ProducesResponseTypeAttribute(400));
                    o.Filters.Add(new ProducesResponseTypeAttribute(500));
                }).AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddEntityFrameworkMySQL().AddDbContext<InvestmentDbContext>(opt =>
            {
                opt.UseMySQL(this.configuration.GetValue<string>("ConnectionStrings"));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDapper, Dapper>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                //Do Something...
            }

            app.UseRouting();
            app.UseCorsExtension();
            app.UseRateLimiter();
            app.UseAuthentication();
			app.UseAuthorization();
            app.UseHealthChecksExtension();
            app.UseResponseCompression();
            app.UseSwaggerExtension(provider);
       
            app.UseExceptionHandler(errorPipeline =>
            {
                errorPipeline.UseExceptionHandlerMiddleware(this.configuration.GetValue("AppSettings:IncludeErrorDetailInResponse", false));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireRateLimiting("fixed-by-ip");
                endpoints.MapControllers().RequireRateLimiting("fixed-by-ip");
            });
        }
    }
}
