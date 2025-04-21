using Autofac;
using Autopark.Common;
using Autopark.Common.Extensions;
using FluentValidation;
using MediatR;

namespace Autopark.PublicApi.Bl;

public class PublicApiBlAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var assembly = GetType().Assembly;

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IValidator<>))
            .AsSelf()
            .InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IValidator<>))
            .AsSelf()
            .InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IPipelineBehavior<,>))
            .AsSelf()
            .InstancePerLifetimeScope();

        OpenGenericRegistrationExtensions.RegisterAssemblyOpenGenericTypes(builder, assembly)
            .AssignableToOpenGeneric(typeof(IValidator<>))
            .As(typeof(IValidator<>))
            .As(t => t.AsSelf())
            .InstancePerLifetimeScope();

        builder.RegisterMappers(assembly);
    }
}
