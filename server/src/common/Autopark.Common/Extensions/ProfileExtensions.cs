using System.Reflection;
using AutoMapper;

namespace Autopark.Common.Extensions;

public static class ProfileExtensions
{
    /// <summary>
    ///     Вызов IMemberConfigurationExpression.Ignore() для свойств объекта назначения,
    ///     кроме тех, что есть в указанном типе (не включая базовые)
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="type"></param>
    /// <param name="except"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IMappingExpression<TSource, TDestination> IgnorePropertiesNotContainedInType<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression,
        Type type,
        params string[] except)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var notIgnoreProps = GetOwnProperties(type);
        var destinationProps = GetAllProperties(typeof(TDestination));
        var exceptProps = except ?? Array.Empty<string>();

        foreach (var destinationProp in destinationProps)
        {
            if (exceptProps.Contains(destinationProp))
            {
                continue;
            }

            if (notIgnoreProps.Contains(destinationProp))
            {
                continue;
            }

            expression.ForMember(destinationProp, m => m.Ignore());
        }

        return expression;
    }

    /// <summary>
    ///     Вызов IMemberConfigurationExpression.Ignore() для свойств объекта назначения,
    ///     для тех, что есть в указанном типе (не включая базовые)
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="type"></param>
    /// <param name="except"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IMappingExpression<TSource, TDestination> IgnorePropertiesContainedInType<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression,
        Type type,
        params string[] except)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var ignoreProps = GetOwnProperties(type);
        var destinationProps = GetAllProperties(typeof(TDestination));
        var exceptProps = except ?? Array.Empty<string>();

        foreach (var ignoreProp in ignoreProps)
        {
            if (!destinationProps.Contains(ignoreProp))
            {
                continue;
            }

            if (exceptProps.Contains(ignoreProp))
            {
                continue;
            }

            expression.ForMember(ignoreProp, m => m.Ignore());
        }

        return expression;
    }

    private static string[] GetOwnProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public |
                                  BindingFlags.Instance |
                                  BindingFlags.DeclaredOnly)
                   .Select(x => x.Name)
                   .ToArray();
    }

    private static string[] GetAllProperties(Type type)
    {
        return type.GetProperties()
                   .Select(x => x.Name)
                   .ToArray();
    }
}