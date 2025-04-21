using Autofac;
using Autopark.Common;
using Autopark.PublicApi.Models.Initializers;
using Microsoft.EntityFrameworkCore;

namespace Autopark.PublicApi.Models;

public class PublicApiModelsAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterType<AutoParkDbContextInitializer>()
            .InstancePerLifetimeScope();

        builder.RegisterType<PublicApiDbContext>()
            .As<DbContext>()
            .InstancePerLifetimeScope();
    }
}