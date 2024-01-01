using Autofac;
using CoreLibrary.ORM.EF;
using CoreLibrary.Security;
using CoreLibrary.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Setup
{
	public class InfrastructureMockModule : Module
	{
		public InfrastructureMockModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<InvestmentMockDbContext>()
			 .As<DbContext>()
			 .InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>()
			 .As<IUnitOfWork>()
			 .InstancePerLifetimeScope();

			builder.RegisterType<JwtProvider>()
			 .As<ITokenProvider>()
			 .InstancePerLifetimeScope();
		}
	}
}
