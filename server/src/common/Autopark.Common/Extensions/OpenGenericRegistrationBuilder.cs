using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autopark.Common.Helpers;

namespace Autopark.Common.Extensions;

public class OpenGenericRegistrationBuilder
{
    private readonly ContainerBuilder builder;

    private IEnumerable<IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle>>
        registrationBuilders;

    private IEnumerable<Type> types;

    internal OpenGenericRegistrationBuilder(ContainerBuilder builder, params Assembly[] assemblies)
    {
        this.builder = builder ?? throw new ArgumentNullException(nameof(builder));

        types = assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(type => type.IsGenericTypeDefinition);
    }
    
    public OpenGenericRegistrationBuilder InstancePerLifetimeScope()
    {
        ForEachRegistrationBuilder(b => b.InstancePerDependency());

        return this;
    }

    public OpenGenericRegistrationBuilder AssignableToOpenGeneric(Type openGenericType)
    {
        if (!openGenericType.NotNull(nameof(openGenericType)).IsGenericTypeDefinition)
        {
            throw new ArgumentException("Open generic type expected", nameof(openGenericType));
        }

        types = types.Where(type =>
            type.IsSubclassOfOpenGeneric(openGenericType) ||
            type.IsSubclassOfOpenGenericInterface(openGenericType));

        return this;
    }

    public OpenGenericRegistrationBuilder As(Type openGenericType)
    {
        if (openGenericType == null)
        {
            throw new ArgumentNullException(nameof(openGenericType));
        }

        if (!openGenericType.IsGenericTypeDefinition)
        {
            throw new ArgumentException("Open generic type expected", nameof(openGenericType));
        }

        ForEachRegistrationBuilder(b => b.As(openGenericType));

        return this;
    }

    public OpenGenericRegistrationBuilder As(
        Action<IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        ForEachRegistrationBuilder(action);

        return this;
    }
    
    private void ForEachRegistrationBuilder(
        Action<IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle>> action)
    {
        SafeCreateRegistrationBuilders();

        foreach (var registrationBuilder in registrationBuilders)
        {
            action(registrationBuilder);
        }
    }

    private void SafeCreateRegistrationBuilders()
    {
        if (registrationBuilders is null)
        {
            registrationBuilders = types.Select(builder.RegisterGeneric);
        }
    }
}
