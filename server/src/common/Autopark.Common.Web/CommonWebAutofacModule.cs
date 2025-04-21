using Autofac;
using Autopark.Common.Bl.Mappers;
using Autopark.Common.Mapping;
using Autopark.Common.Security;
using Autopark.Common.Web.Middleware;
using Autopark.Common.Web.Security;

namespace Autopark.Common.Web;

public class CommonWebAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType(typeof(ErrorDetailsHandlerMiddleware))
            .AsSelf()
            .InstancePerLifetimeScope();
        
        builder.RegisterComposite<CompositeUserInfoProvider, IUserInfoProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(DefaultMapper<,>))
            .As(typeof(IMapper<,>), typeof(IAfterMapper<,>), typeof(IExtendedMapper<,>))
            .InstancePerLifetimeScope();

        builder.RegisterType(typeof(DefaultMapper)).As(typeof(IMapper)).SingleInstance();
    }
}
