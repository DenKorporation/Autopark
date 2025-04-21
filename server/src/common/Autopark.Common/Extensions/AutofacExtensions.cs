using System.Reflection;
using Autofac;
using Autopark.Common.Mapping;

namespace Autopark.Common.Extensions;

public static class AutofacExtensions
{
    public static void RegisterMappers(this ContainerBuilder builder, Assembly assembly)
    {
        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IMapper<,>))
            .AsClosedTypesOf(typeof(IExtendedMapper<,>))
            .AsClosedTypesOf(typeof(IAfterMapper<,>))
            .InstancePerLifetimeScope();
    }
}
