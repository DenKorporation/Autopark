using Autofac;
using Autopark.Common;
using Autopark.Dal.Core.Extensions;

namespace Autopark.PublicApi.Dal;

public class PublicApiDalAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var assembly = GetType().Assembly;

        builder.AddCoreFilterConverters(assembly);
        builder.RegisterRepositories(assembly);
    }
}
