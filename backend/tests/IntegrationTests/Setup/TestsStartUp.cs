using Asp.Versioning.ApiExplorer;
using Autofac;
using CoreLibrary.ORM.Dapper;
using CoreLibrary.ORM.EF;
using CoreLibrary.Security;
using CoreLibrary.UnitOfWork;
using Invest.Application.Behaviors;
using Invest.Infrastructure.Bootstrap.AutofacModules;
using Invest.Infrastructure.Bootstrap.Extensions.ApplicationBuilder;
using Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection;
using Invest.Infrastructure.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace IntegrationTests.Setup
{
	public class TestsStartup
    {
		protected const string JwtPolicy = "JwtPolicy";
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment env;

		public TestsStartup(IConfiguration configuration, IWebHostEnvironment env)
		{
			this.configuration = configuration;
			this.env = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(TestAuthenticationOptions.Scheme).AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestAuthenticationOptions.Scheme, null);

			services.AddCorsExtension();
			services.AddApiVersionExtension("2.0");
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

			services.AddDbContext<InvestmentMockDbContext>(opt =>
			{
				opt.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			});	

			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IDapper, Dapper>();
		}

		public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
		{
			app.UseCorsExtension();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHealthChecksExtension();
			app.UseResponseCompression();
			app.UseSwaggerExtension(provider);

			app.UseExceptionHandler(errorPipeline =>
			{
				errorPipeline.UseExceptionHandlerMiddleware(true);
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapControllers();
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			_ = builder.RegisterType<CurrentUserAccessor>()
				.As<ICurrentUserAccessor>()
				.InstancePerLifetimeScope();

			builder.RegisterModule(new InfrastructureMockModule());
			builder.RegisterModule(new MediatorModule(configuration));
		}
	}
}
