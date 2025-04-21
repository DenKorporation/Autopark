using Autopark.Dal.Core.Filters;

namespace Autopark.Dal.Core.Queries;

/// <summary>
///     Базовый класс для запросов
/// </summary>
/// <typeparam name="TFilter">Тип фильтра</typeparam>
/// <typeparam name="TResult">Тип результата</typeparam>
public class QueryBase<TFilter, TResult> : QueryFilter<TFilter>, IResultQuery<TResult>
    where TFilter : class, new()
    where TResult : class, new()
{
    /// <inheritdoc />
    public QueryBase(QueryFilter<TFilter> baseFilter)
    {
        Filter = baseFilter.Filter;
        Includes = baseFilter.Includes;
        Order = baseFilter.Order;
        Skip = baseFilter.Skip;
        Take = baseFilter.Take;
        OnlyCount = baseFilter.OnlyCount;
        WithCount = baseFilter.WithCount;
        EnablePrivateMode = baseFilter.EnablePrivateMode;
    }

    /// <inheritdoc />
    public QueryBase()
    {
    }
}
