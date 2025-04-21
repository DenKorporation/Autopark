using Autofac;
using Autopark.Common.Attributes;
using Autopark.Common.Helpers;

namespace Autopark.Common;

public abstract class AutofacModuleBase : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = GetType().Assembly;

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.HasCustomAttribute<ServiceAsInterfacesAttribute>()
                        && !t.IsInterface && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .AsImplementedInterfaces()
            .WithMetadataFrom<ServiceAsInterfacesAttribute>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.HasCustomAttribute<ServiceAsSelfAttribute>()
                        && !t.IsInterface && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
