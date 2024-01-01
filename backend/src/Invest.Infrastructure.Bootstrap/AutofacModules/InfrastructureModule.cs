using Autofac;
using CoreLibrary.ORM.Dapper;
using CoreLibrary.ORM.EF;
using CoreLibrary.Security;
using CoreLibrary.UnitOfWork;
using Invest.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Invest.Infrastructure.Bootstrap.AutofacModules
{
    public class InfrastructureModule(IConfiguration configuration) : Module
    {
        private readonly IConfiguration configuration = configuration;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InvestmentDbContext>()
             .As<DbContext>()
             .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
             .As<IUnitOfWork>()
             .InstancePerLifetimeScope();

            builder.Register(c => new MySqlConnection(this.configuration.GetValue<string>("ConnectionStrings")))
             .As<IDbConnection>()
             .InstancePerLifetimeScope();

            builder.RegisterType<Dapper>()
             .As<IDapper>()
             .InstancePerLifetimeScope();

            builder.RegisterType<JwtProvider>()
             .As<ITokenProvider>()
             .InstancePerLifetimeScope();
        }
    }
}
