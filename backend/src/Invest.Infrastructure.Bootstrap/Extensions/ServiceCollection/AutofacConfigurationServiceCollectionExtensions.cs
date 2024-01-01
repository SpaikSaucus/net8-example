using Autofac;
using CoreLibrary.Security;
using Invest.Infrastructure.Bootstrap.AutofacModules;
using Microsoft.Extensions.Configuration;

namespace Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class AutofacConfigurationServiceCollectionExtensions
    {
        public static void AddAutofacExtension(this ContainerBuilder builder, IConfiguration configuration)
        {
            _ = builder.RegisterType<CurrentUserAccessor>()
                .As<ICurrentUserAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new InfrastructureModule(configuration));
            builder.RegisterModule(new MediatorModule(configuration));
        }
    }
}
