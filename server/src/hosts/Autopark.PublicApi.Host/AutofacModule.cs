using Autofac;
using Autopark.Common;
using Autopark.Common.Web.Security;

namespace Autopark.PublicApi.Host;

public class AutofacModule : AutofacModuleBase
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = GetType().Assembly;

        builder.RegisterAssemblyTypes(assembly)
            .AssignableTo<IPermissionRule>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
