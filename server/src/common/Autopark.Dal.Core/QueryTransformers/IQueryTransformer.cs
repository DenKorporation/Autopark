using Autopark.Common.Domain;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Microsoft.EntityFrameworkCore;

namespace Autopark.Dal.Core.QueryTransformers;

/// <summary>
///     Класс для формирования и модификации запросов к БД
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IQueryTransformer<TEntity>
    where TEntity : class, IEntityBase, new()
{
    /// <summary>
    ///     Сформировать запрос по переданному фильтру.
    ///     Применяется сортировка и пагинация
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <param name="filter"></param>
    /// <param name="querySource"></param>
    /// <returns></returns>
    public IQueryable<TEntity> CreateQueryable<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource)
        where TFilter : class, new();

    /// <summary>
    ///     Применить сортировку к запросу
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="queryable"></param>
    /// <param name="dbContext">Необходим для сортировки по-умолчанию на основании PK</param>
    /// <returns></returns>
    public IQueryable<TEntity> ApplyOrder(
        QueryFilterBase filter,
        IQueryable<TEntity> queryable,
        DbContext dbContext);

    /// <summary>
    ///     Применить пагинацию к запросу
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="queryable"></param>
    public IQueryable<TEntity> ApplyPaging(
        QueryFilterBase filter,
        IQueryable<TEntity> queryable);

    /// <summary>
    ///     Получить запрос и кол-во данных по фильтру
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="querySource"></param>
    /// <param name="dbContext">Необходим для сортировки по-умолчанию на основании PK</param>
    /// <param name="ct"></param>
    public Task<QueryResult<TEntity>> GetQueryableResultAsync<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource,
        DbContext dbContext,
        CancellationToken ct = default)
        where TFilter : class, new();

    /// <summary>
    ///     Получить запрос и кол-во данных по фильтру
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="querySource"></param>
    /// <param name="dbContext">Необходим для сортировки по-умолчанию на основании PK</param>
    public QueryResult<TEntity> GetQueryableResult<TFilter>(
        QueryFilter<TFilter> filter,
        Func<IQueryable<TEntity>> querySource,
        DbContext dbContext)
        where TFilter : class, new();
}