using Autofac;
using Autopark.Common.Bl.Validators;
using Autopark.Common.Extensions;
using Autopark.Common.Helpers;
using Autopark.Common.Validations;
using Autopark.Dal.Core.Extensions;
using Autopark.Dal.Core.FilterConverters;
using FluentValidation;

namespace Autopark.Common.Bl;

public class BlCommonAutofacModule : AutofacModuleBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var assembly = GetType().Assembly;

        builder.RegisterAssemblyTypes(typeof(BlCommonAutofacModule).Assembly)
               .Where(t => t.IsSubclassOfOpenGeneric(typeof(FilterConverterBase<,>)))
               .AsClosedTypesOf(typeof(FilterConverterBase<,>)).InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(IValidator<>))
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(DefaultModelValidator<>))
               .As(typeof(IModelValidator<>))
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(QueryFilterValidator<>))
            .As(typeof(IValidator<>))
            .InstancePerLifetimeScope();

        builder.RegisterMappers(assembly);

        builder.RegisterRepositories(assembly);
    }
}