using System.Linq.Dynamic.Core;
using Autopark.Common.Domain;
using Autopark.Common.Extensions;
using Autopark.Dal.Core.Extensions;
using Autopark.Dal.Core.FilterConverters;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Providers;
using Autopark.Dal.Core.Queries;
using Microsoft.EntityFrameworkCore;

namespace Autopark.Dal.Core.QueryTransformers;

/// <summary>
///     Базовый класс для репозиториев
/// </summary>
public sealed class QueryTransformer<TEntity> : IQueryTransformer<TEntity>
    where TEntity : class, IEntityBase, new()
{
    private const int MaxTake = 1000;

    private readonly IFilterConverter.Factory converterFactory;
    private readonly IEntityInfoProvider entityInfoProvider;

    public QueryTransformer(
        IFilterConverter.Factory converterFactory,
        IEntityInfoProvider entityInfoProvider)
    {
        this.converterFactory = converterFactory;
        this.entityInfoProvider = entityInfoProvider;
    }

    /// <inheritdoc />
    public IQueryable<TEntity> CreateQueryable<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource)
        where TFilter : class, new()
    {
        var converter = converterFactory(typeof(TEntity), typeof(TFilter));
        var expr = converter.CreateFilterExpression<TEntity, TFilter>(filter);

        var queryable = querySource().Where(expr);

        if (filter.Includes.NotNullOrEmpty())
        {
            foreach (var include in filter.Includes)
            {
                queryable.Include(include);
            }
        }

        return queryable;
    }

    /// <inheritdoc />
    public IQueryable<TEntity> ApplyOrder(QueryFilterBase filter, IQueryable<TEntity> queryable, DbContext dbContext)
    {
        if (filter.DoNotOrder)
        {
            return queryable;
        }

        if (filter.Order?.Any() == true)
        {
            var order = filter.Order.ToString(
                ",",
                ord =>
                    $"it.{ord.Field}{(ord.Desc == true ? " descending" : null)}");

            return queryable.OrderBy(order);
        }

        if (filter.Take == 1)
        {
            return queryable;
        }

        return queryable.OrderByDefaultSortField(entityInfoProvider);
    }

    /// <inheritdoc />
    public IQueryable<TEntity> ApplyPaging(
        QueryFilterBase filter,
        IQueryable<TEntity> queryable)
    {
        if (filter.Skip.HasValue)
        {
            queryable = queryable.Skip(filter.Skip.Value);
        }

        if (!filter.Take.HasValue || filter.Take > MaxTake)
        {
            filter.Take = MaxTake;
        }

        queryable = queryable.Take(filter.Take.Value);

        return queryable;
    }

    /// <inheritdoc />
    public async Task<QueryResult<TEntity>> GetQueryableResultAsync<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource,
        DbContext dbContext,
        CancellationToken ct = default)
        where TFilter : class, new()
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter), "Попытка передать пустую ссылку на фильтр");
        }

        var queryable = CreateQueryable(filter, querySource);

        var result = new QueryResult<TEntity>();

        if (filter.OnlyCount || filter.WithCount == true)
        {
            result.Count = await queryable.CountAsync(ct);
        }

        if (result.Count == 0)
        {
            result.Items = Enumerable.Empty<TEntity>().AsQueryable();
        }

        if (filter.OnlyCount)
        {
            return result;
        }

        queryable = ApplyOrder(filter, queryable, dbContext);
        queryable = ApplyPaging(filter, queryable);

        result.Items = queryable;

        return result;
    }

    /// <inheritdoc />
    public QueryResult<TEntity> GetQueryableResult<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource,
        DbContext dbContext)
        where TFilter : class, new()
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter), "Попытка передать пустую ссылку на фильтр");
        }

        var queryable = CreateQueryable(filter, querySource);

        var result = new QueryResult<TEntity>();

        if (filter.OnlyCount || filter.WithCount == true)
        {
            result.Count = queryable.Count();
        }

        if (result.Count == 0)
        {
            result.Items = Enumerable.Empty<TEntity>().AsQueryable();
        }

        if (filter.OnlyCount)
        {
            return result;
        }

        queryable = ApplyOrder(filter, queryable, dbContext);
        queryable = ApplyPaging(filter, queryable);

        result.Items = queryable;

        return result;
    }
}
