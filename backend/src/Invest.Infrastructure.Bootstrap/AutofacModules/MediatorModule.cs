using Autofac;
using FluentValidation;
using Invest.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Invest.Infrastructure.Bootstrap.AutofacModules
{
    public class MediatorModule(IConfiguration configuration) : Module
    {
        private readonly IConfiguration configuration = configuration;

        protected override void Load(ContainerBuilder builder)
        {
            //Discover "Application" service layer validations and auto-register with MediatR pipeline
            builder.RegisterAssemblyTypes(typeof(ValidatorBehavior<,>).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();

            if (this.configuration.GetValue("CommandLoggingEnabled", false))
            {
                builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            }

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
