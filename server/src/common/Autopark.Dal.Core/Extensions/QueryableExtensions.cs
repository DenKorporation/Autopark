using Autopark.Common.Domain;
using Autopark.Common.Extensions;
using Autopark.Common.Security;
using Autopark.Dal.Core.Providers;

namespace Autopark.Dal.Core.Extensions;

/// <summary>
///     Расширения для IQueryable
/// </summary>
public static class QueryableExtensions
{
    public static IQueryable<TEntity> FilterByGroup<TEntity>(
        this IQueryable<TEntity> queryable,
        IUserInfoProvider userInfoProvider)
        where TEntity : class, IEntityBase
    {
        return queryable.Where(x => x.GroupId == userInfoProvider.GetGroupId());
    }

    /// <summary>
    ///     Сортировка по умолчанию по возрастанию
    /// </summary>
    /// <param name="queryable">IQueryable</param>
    /// <param name="provider"></param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <returns></returns>
    public static IOrderedQueryable<TEntity> OrderByDefaultSortField<TEntity>(
        this IQueryable<TEntity> queryable,
        IEntityInfoProvider provider)
        where TEntity : class, IEntityBase
    {
        return OrderBy(queryable, provider, false);
    }

    /// <summary>
    ///     Сортировка по умолчанию по убыванию
    /// </summary>
    /// <param name="queryable">IQueryable</param>
    /// <param name="provider"></param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <returns></returns>
    public static IOrderedQueryable<TEntity> OrderByDescendingByDefaultSortField<TEntity>(
        this IQueryable<TEntity> queryable,
        IEntityInfoProvider provider)
        where TEntity : class, IEntityBase
    {
        return OrderBy(queryable, provider, true);
    }

    private static IOrderedQueryable<TEntity> OrderBy<TEntity>(
        IQueryable<TEntity> queryable,
        IEntityInfoProvider provider,
        bool desc)
        where TEntity : class, IEntityBase
    {
        var sortFields = GetDefaultSortFieldExpression<TEntity>(provider, desc);
        return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(queryable, sortFields);
    }

    private static string GetDefaultSortFieldExpression<TEntity>(IEntityInfoProvider provider, bool desc)
        where TEntity : class, IEntityBase
    {
        return provider.GetDefaultSortFields<TEntity>()
            .ToString(",", key => $"it.{key}{(desc ? " descending" : null)}");
    }
}