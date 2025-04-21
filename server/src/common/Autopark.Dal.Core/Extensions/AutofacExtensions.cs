using System.Reflection;
using Autofac;
using Autofac.Core;
using Autopark.Common.Helpers;
using Autopark.Dal.Core.FilterConverters;
using Autopark.Dal.Core.Repositories;

namespace Autopark.Dal.Core.Extensions;

public static class AutofacExtensions
{
    public static void AddCoreFilterConverters(this ContainerBuilder builder, Assembly assembly)
    {
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.IsSubclassOfOpenGeneric(typeof(FilterConverterBase<,>)))
            .AsClosedTypesOf(typeof(FilterConverterBase<,>)).InstancePerLifetimeScope();
    }

    public static void RegisterRepositories(this ContainerBuilder builder, Assembly assembly)
    {
        builder.RegisterAssemblyTypes(assembly).AssignableTo<IRepository>()
            .As(type => type.GetImmediateInterfaces())
            .InstancePerLifetimeScope()
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IRepository),
                    (pi, ctx) => ctx.Resolve<DefaultRepository>()));

        builder.RegisterType<DefaultRepository>()
            .As<IRepository>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}