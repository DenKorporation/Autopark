using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Autopark.Common.Expressions;
using Autopark.Common.Extensions;
using Autopark.Common.Helpers;
using Autopark.Dal.Core.Extensions;
using Autopark.Dal.Core.Filters;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.FilterConverters;

public abstract class FilterConverterBase<TModel, TFilter> : IFilterConverter
    where TFilter : class, new()
    where TModel : class
{
    protected static readonly Expression<Func<TModel, bool>> DoNothing =
        PredicateBuilder.New<TModel>(true).DefaultExpression;

    private static readonly IDictionary<Type, IDictionary<string, MethodInfo>> TypeMethods =
        new Dictionary<Type, IDictionary<string, MethodInfo>>
        {
            {
                typeof(Enumerable),
                new Dictionary<string, MethodInfo>
                {
                    {
                        nameof(Enumerable.Contains),
                        typeof(Enumerable).GetMethods()
                            .First(m => m.Name == nameof(Enumerable.Contains) &&
                                        m.GetParameters().Length == 2)
                    },
                }
            },
            {
                typeof(string),
                new Dictionary<string, MethodInfo>
                {
                    {
                        nameof(string.Contains),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                    },
                    {
                        nameof(string.StartsWith),
                        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })
                    },
                    {
                        nameof(string.EndsWith),
                        typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })
                    },
                }
            },
        };

    private readonly ILogger<FilterConverterBase<TModel, TFilter>> logger;

    protected FilterConverterBase(ILogger<FilterConverterBase<TModel, TFilter>> logger)
    {
        this.logger = logger;
        var filterProps = typeof(TFilter).GetProperties();
        var entityProps = typeof(TModel).GetProperties().Select(s => s.Name).ToArray();
        var unmatched = filterProps.Select(s => s.Name).Where(f => !entityProps.Contains(f));

        UnmatchedProps = new List<string>(unmatched);
        ExplicitHandlers = new Dictionary<string, FilterPropertyHandler>();
    }

    protected delegate Expression<Func<TModel, bool>> FilterPropertyHandler(
        PropertyInfo prop,
        object filterValue,
        TFilter filter);

    protected delegate Expression<Func<TModel, bool>> SimpleFilterPropertyHandler(
        PropertyInfo prop,
        object filterValue);

    protected IList<string> UnmatchedProps { get; set; }

    protected IDictionary<string, FilterPropertyHandler> ExplicitHandlers { get; set; }

    public static Expression<Func<TModel, bool>> ApplyStringFilter(
        Expression<Func<TModel, bool>> mainExpression,
        Expression<Func<TModel, string>> prop,
        StringFilter filter)
    {
        if (filter == null || !filter.HasValue())
        {
            return mainExpression;
        }

        var lam = GetStringExpression<TModel, string>(prop, filter);

        return ParameterVisitor.AndAlso(lam, mainExpression);
    }

    public static Expression<Func<TModel, bool>> ApplyListFilter<TProperty>(
        Expression<Func<TModel, bool>> mainExpression,
        Expression<Func<TModel, TProperty>> prop,
        ListFilter<TProperty> values)
    {
        if (values == null || !values.HasValue() || values.Any(p => 0.Equals(p)))
        {
            return mainExpression;
        }

        var lam = GetListExpression(prop, values);

        return mainExpression.And(lam);
    }

    public static Expression<Func<TModel, bool>> ApplyRangeFilter<TProperty>(
        Expression<Func<TModel, bool>> mainExpression,
        Expression<Func<TModel, TProperty?>> prop,
        RangeFilter<TProperty> values)
        where TProperty : struct
    {
        if (values == null || !values.HasValue())
        {
            return mainExpression;
        }

        var lam = GetRangeExpression(prop, (RangeFilterEx<TProperty>)values);

        return mainExpression.And(lam);
    }

    public static Expression<Func<TModel, bool>> ApplyRangeFilter<TProperty>(
        Expression<Func<TModel, bool>> mainExpression,
        Expression<Func<TModel, TProperty?>> prop,
        RangeFilterEx<TProperty> values)
        where TProperty : struct
    {
        if (values == null || !values.HasValue())
        {
            return mainExpression;
        }

        var lam = GetRangeExpression(prop, values);

        return mainExpression.And(lam);
    }

    Expression<Func<TModel1, bool>> IFilterConverter.CreateFilterExpression<TModel1, TFilter1>(
        QueryFilter<TFilter1> filter)
    {
        var expression =
            (Expression<Func<TModel1, bool>>)(object)CreateFilterExpression((QueryFilter<TFilter>)(object)filter);
        return expression;
    }

    public virtual Expression<Func<TModel, bool>> CreateFilterExpression(QueryFilter<TFilter> filter)
    {
        var res = PredicateBuilder.New<TModel>(true);

        foreach (var filterProp in typeof(TFilter).GetProperties())
        {
            var sourceValue = filterProp.GetValue(filter.Filter);

            if (ExplicitHandlers.ContainsKey(filterProp.Name))
            {
                var handler = ExplicitHandlers[filterProp.Name](filterProp, sourceValue, filter.Filter);
                if (handler != (Expression<Func<TModel, bool>>)DoNothing)
                {
                    res = filter.FilterMode == QueryFilterMode.Or ? res.Or(handler) : res.And(handler);
                }

                continue;
            }

            var indexProp = typeof(TModel).GetProperty(filterProp.Name);

            var defHandler = CreateExpressionFromFilterProperty(filterProp, indexProp, sourceValue);
            if (defHandler != (Expression<Func<TModel, bool>>)DoNothing)
            {
                res = filter.FilterMode == QueryFilterMode.Or ? res.Or(defHandler) : res.And(defHandler);
            }
        }

        if (UnmatchedProps.NotNullOrEmpty())
        {
            throw new InvalidOperationException(
                $"Filter '{typeof(TFilter)}' properties " +
                $"'{UnmatchedProps.ToString(", ", f => f)}'" +
                $" are not mapped to entity '{typeof(TModel)}' fields");
        }

        var optimized = ExpressionOptimizer.tryVisit(res);
        return (Expression<Func<TModel, bool>>)optimized;
    }

    protected static Expression<Func<TModel, bool>> GetListExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        IListFilter listFilter)
    {
        if (!listFilter.HasValue())
        {
            return DoNothing;
        }

        var containsMethod = TypeMethods[typeof(Enumerable)][nameof(Enumerable.Contains)];
        var constant = Expression.Constant(listFilter.Cast<TProperty>().ToArray());

        Expression body =
            Expression.Call(containsMethod.MakeGenericMethod(typeof(TProperty)), constant, prop.Body);

        if (listFilter.IsInverted)
        {
            body = Expression.Not(body);
        }

        var lam = (Expression<Func<TModel, bool>>)Expression.Lambda(body, prop.Parameters);
        return lam;
    }

    protected static Expression<Func<TModel, bool>> GetStringExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        StringFilter filter)
    {
        return GetStringExpression<TModel, TProperty>(prop, filter);
    }

    protected static Expression<Func<TM, bool>> GetStringExpression<TM, TProperty>(
        Expression<Func<TM, TProperty>> prop,
        StringFilter filter)
    {
        if (!filter.HasValue())
        {
            return (Expression<Func<TM, bool>>)(object)DoNothing;
        }

        Expression body = null;
        MethodInfo method = null;

        var constant = Expression.Constant(filter);
        var value = Expression.MakeMemberAccess(constant, filter.GetMemberInfo(f => f.Value));
        var memberBody = prop.Body;

        if (filter.Contains)
        {
            method = TypeMethods[typeof(string)][nameof(string.Contains)];
        }
        else if (filter.StartsWith)
        {
            method = TypeMethods[typeof(string)][nameof(string.StartsWith)];
        }
        else if (filter.EndsWith)
        {
            method = TypeMethods[typeof(string)][nameof(string.EndsWith)];
        }

        if (method == null)
        {
            if (filter.IsNull)
            {
                body = Expression.Equal(memberBody, Expression.Constant(null));
            }
            else if (filter.IsNotNull)
            {
                body = Expression.NotEqual(memberBody, Expression.Constant(null));
            }
            else
            {
                body = Expression.Equal(memberBody, value);
            }
        }
        else
        {
            body = Expression.NotEqual(memberBody, Expression.Constant(null)); // null check
            body = Expression.AndAlso(body, Expression.Call(memberBody, method, value));
        }

        if (filter.IsInverted)
        {
            body = Expression.Not(body);
        }

        return (Expression<Func<TM, bool>>)Expression.Lambda(body, prop.Parameters);
    }

    protected static Expression<Func<TModel, bool>> GetNullableExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        INullableFilter value)
    {
        var memberBody = prop.Body;

        Expression body = null;

        if (!typeof(TProperty).IsNullable())
        {
            if (value.IsNull)
            {
                return a => false;
            }

            return DoNothing;
        }

        if (value.IsNull)
        {
            body = Expression.Equal(memberBody, Expression.Constant(null, typeof(TProperty)));
        }
        else if (value.IsNotNull)
        {
            body = Expression.NotEqual(memberBody, Expression.Constant(null, typeof(TProperty)));
        }

        if (body == null)
        {
            return DoNothing;
        }

        var lam = (Expression<Func<TModel, bool>>)Expression.Lambda(body, prop.Parameters);
        return lam;
    }

    protected static Expression<Func<TModel, bool>> GetConstantExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        object value)
    {
        var valueExpr = ExpressionExtensions.CreateClousre(value, typeof(TProperty));

        var body = Expression.Equal(prop.Body, valueExpr);
        var lam = (Expression<Func<TModel, bool>>)Expression.Lambda(body, prop.Parameters);
        return lam;
    }

    protected static Expression<Func<TModel, bool>> GetRangeExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        IRangeFilter rangeFilter)
    {
        return GetRangeExpression(prop, rangeFilter.ToRangeFilterEx());
    }

    protected static Expression<Func<TModel, bool>> GetRangeExpression<TProperty>(
        Expression<Func<TModel, TProperty>> prop,
        IRangeFilterEx rangeFilter)
    {
        var param = Expression.Parameter(typeof(TModel));
        var memberBody = new ParameterVisitor(prop.Parameters, new[] { param }).Visit(prop.Body);

        var constant = Expression.Constant(rangeFilter);
        var start = Expression.Property(constant, nameof(IRangeFilterEx.Start));
        var end = Expression.Property(constant, nameof(IRangeFilterEx.End));

        if (!memberBody.Type.IsNullable()
            && start.Type.IsNullable())
        {
            memberBody = Expression.Convert(memberBody, start.Type);
        }

        var more = rangeFilter.StartIsExclusive
            ? Expression.GreaterThan(memberBody, start)
            : Expression.GreaterThanOrEqual(memberBody, start);

        var less = rangeFilter.EndIsExclusive
            ? Expression.LessThan(memberBody, end)
            : Expression.LessThanOrEqual(memberBody, end);

        Expression body = null;

        if (rangeFilter.Start != null && rangeFilter.End != null)
        {
            body = Expression.AndAlso(more, less);
        }
        else
        {
            if (rangeFilter.Start != null)
            {
                if (rangeFilter.UseAsSingleValue)
                {
                    Expression.Equal(memberBody, Expression.Constant(start, typeof(TProperty)));
                }
                else
                {
                    body = more;
                }
            }

            if (rangeFilter.End != null)
            {
                body = less;
            }
        }

        if (body == null)
        {
            return DoNothing;
        }

        var lam = (Expression<Func<TModel, bool>>)Expression.Lambda(body, param);
        return lam;
    }

    protected void HandleField<TProp>(Expression<Func<TFilter, TProp>> filterProp, FilterPropertyHandler handler)
    {
        var member = ReflectionHelper.GetPropertyInfo(filterProp);
        try
        {
            UnmatchedProps.Remove(member.Name);
        }
        catch (Exception e)
        {
            throw new AggregateException($"Unable add filter field handler: {filterProp}, property already handled",
                e);
        }

        ExplicitHandlers[member.Name] = handler;
    }

    protected void HandleField<TProp>(
        Expression<Func<TFilter, TProp>> filterProp,
        SimpleFilterPropertyHandler handler)
    {
        var member = ReflectionHelper.GetPropertyInfo(filterProp);
        try
        {
            UnmatchedProps.Remove(member.Name);
        }
        catch (Exception e)
        {
            throw new AggregateException($"Unable add filter field handler: {filterProp}, property already handled",
                e);
        }

        ExplicitHandlers[member.Name] = (prop, value, filter) => handler(prop, value);
    }

    protected void HandleField<TProp, TMProp>(
        Expression<Func<TFilter, TProp>> filterProp,
        Expression<Func<TModel, TMProp>> entityProp)
    {
        var member = ReflectionHelper.GetPropertyInfo(filterProp);
        try
        {
            UnmatchedProps.Remove(member.Name);
        }
        catch (Exception e)
        {
            throw new AggregateException($"Unable add filter field handler: {filterProp}, property already handled",
                e);
        }

        ExplicitHandlers[member.Name] =
            (prop, value, filter) => CreateExpressionFromFilterProperty(prop, entityProp, value);
    }

    protected Expression<Func<TModel, bool>> CreateExpressionFromFilterProperty(
        PropertyInfo filterProp,
        PropertyInfo entityProp,
        object sourceValue)
    {
        if (entityProp == null)
        {
            return DoNothing;
        }

        var param = Expression.Parameter(typeof(TModel));
        var body = Expression.MakeMemberAccess(param, entityProp);
        var lam = Expression.Lambda(body, param);

        var ret = ReflectionHelper.CallGenericMethod(this,
            nameof(CreateExpressionFromFilterProperty),
            entityProp.PropertyType,
            new object[] { filterProp, lam, sourceValue });

        return (Expression<Func<TModel, bool>>)ret;
    }

    protected virtual Expression<Func<TModel, bool>> CreateExpressionFromFilterProperty<TProperty>(
        PropertyInfo filterProp,
        Expression<Func<TModel, TProperty>> entityProp,
        object sourceValue)
    {
        if (sourceValue == null || sourceValue as string == string.Empty)
        {
            return DoNothing;
        }

        var propType = filterProp.PropertyType;

        try
        {
            if (!propType.IsInstanceOfType(sourceValue)
                && !(sourceValue is IEnumerable && propType.GetGenericElementType().IsInstanceOfType(sourceValue)))
            {
                throw new InvalidOperationException(string.Format(
                    "Filter property type: '{0}' does not match with source value type: '{1}'",
                    propType.Name,
                    sourceValue.GetType().Name));
            }

            if (sourceValue is IEnumerable
                && !(sourceValue is string)
                && !(sourceValue is INullableFilter))
            {
                var valType = propType.GetGenericElementType();
                propType = valType;
            }

            if (typeof(INullableFilter).IsAssignableFrom(propType))
            {
                var nullableFilter = (INullableFilter)sourceValue;
                var ret = GetNullableExpression(entityProp, nullableFilter);
                if (ret != (Expression<Func<TModel, bool>>)DoNothing)
                {
                    return ret;
                }
            }

            if (typeof(IListFilter).IsAssignableFrom(propType))
            {
                var listFilter = (IListFilter)sourceValue;
                return GetListExpression(entityProp, listFilter);
            }

            if (typeof(StringFilter).IsAssignableFrom(propType))
            {
                var stringFilter = (StringFilter)sourceValue;
                return GetStringExpression<TModel, TProperty>(entityProp, stringFilter);
            }

            if (typeof(IRangeFilter).IsAssignableFrom(propType))
            {
                var rangeFilter = (IRangeFilter)sourceValue;
                return GetRangeExpression(entityProp, rangeFilter);
            }

            if (typeof(IRangeFilterEx).IsAssignableFrom(propType))
            {
                var rangeFilter = (IRangeFilterEx)sourceValue;
                return GetRangeExpression(entityProp, rangeFilter);
            }

            if (sourceValue is IEnumerable enumerable && !(sourceValue is string))
            {
                var start = PredicateBuilder.New<TModel>(true);

                foreach (var val in enumerable)
                {
                    start = start.Or(CreateExpressionFromFilterProperty(filterProp, entityProp, val));
                }

                return start ?? DoNothing;
            }

            return GetConstantExpression(entityProp, sourceValue);
        }
        catch (Exception e)
        {
            var format = "Unable to create filter {2} for property: {0}, model field: {1}";
            logger.LogError(format, filterProp.Name, entityProp.Name, GetType());
            throw new AggregateException(string.Format(format, filterProp.Name, entityProp.Body, GetType().Name),
                e);
        }
    }

    protected void IgnoreField<TProp>(Expression<Func<TFilter, TProp>> filterProp)
    {
        HandleField(filterProp, (prop, filterInstance) => DoNothing);
    }
}
