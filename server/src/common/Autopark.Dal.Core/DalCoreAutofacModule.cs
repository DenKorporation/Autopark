using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autopark.Common;
using Autopark.Dal.Core.FilterConverters;
using Autopark.Dal.Core.QueryTransformers;
using Autopark.Dal.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Autopark.Dal.Core;

public class DalCoreAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterGeneric(typeof(IdGuidFilterConverter<>))
            .As(typeof(FilterConverterBase<,>))
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterComponent(RegistrationBuilder.ForDelegate(typeof(IFilterConverter), (ctx, @params) =>
        {
            var types = @params.OfType<NamedParameter>().Select(f => (Type)f.Value).ToArray();
            var value = ctx.Resolve(typeof(FilterConverterBase<,>).MakeGenericType(types));
            return (IFilterConverter)value;
        }).CreateRegistration());

        builder.RegisterGeneric(typeof(DefaultRepository<>))
            .As(typeof(IRepository<>))
            .WithProperty(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IServiceProvider),
                    (pi, ctx) => ctx.Resolve<IServiceProvider>()))
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(DefaultDtoRepository<,>))
            .As(typeof(IDtoRepository<,>))
            .WithProperty(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IServiceProvider),
                    (pi, ctx) => ctx.Resolve<IServiceProvider>()))
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(DefaultEditDtoRepository<,>))
            .As(typeof(IEditDtoRepository<,>))
            .WithProperty(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IServiceProvider),
                    (pi, ctx) => ctx.Resolve<IServiceProvider>()))
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<DefaultRepository>()
            .As<IRepository>()
            .WithProperty(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IServiceProvider),
                    (pi, ctx) => ctx.Resolve<IServiceProvider>()))
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(QueryTransformer<>))
            .As(typeof(IQueryTransformer<>))
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
