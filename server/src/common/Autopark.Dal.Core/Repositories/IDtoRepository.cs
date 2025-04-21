using Autopark.Common.Domain;
using Autopark.Common.Mapping;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;

namespace Autopark.Dal.Core.Repositories;

/// <summary>Репозиторий для чтения проекций сущностей</summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
public interface IDtoRepository<TEntity, TDto> : IRepository<TEntity>
    where TEntity : class, IEntityBase, new()
    where TDto : class, new()
{
    /// <summary>Gets the mapper.</summary>
    IMapper<TEntity, TDto> Mapper { get; }

    /// <summary>The map entity.</summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    TDto MapEntity(TEntity entity);

    /// <summary>The query async.</summary>
    /// <param name="queryDto">The query dto.</param>
    /// <returns></returns>
    Task<QueryResultDto<TDto>> QueryDtoAsync<TFilter>(QueryFilter<TFilter> queryDto, CancellationToken ct = default)
        where TFilter : class, new();

    /// <summary>The query.</summary>
    /// <param name="queryDto">The query dto.</param>
    /// <returns></returns>
    QueryResultDto<TDto> QueryDto<TFilter>(QueryFilter<TFilter> queryDto)
        where TFilter : class, new();

    /// <summary>
    ///     Возвращает объект по входящим параметрам.
    /// </summary>
    /// <param name="id">Id сущности</param>
    /// <returns></returns>
    Task<TDto?> GetDtoAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Репозиторий для чтения проекций сущностей</summary>
/// <typeparam name="TEntity"></typeparam>
public interface IDtoRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntityBase, new()
{
    /// <summary>The query async.</summary>
    /// <param name="queryDto">The query dto.</param>
    /// <returns></returns>
    Task<IQueryResultDto<TDto>> QueryDtoAsync<TFilter, TDto>(QueryBase<TFilter, TDto> queryDto, CancellationToken ct = default)
        where TFilter : class, new()
        where TDto : class, new();

    /// <summary>
    ///     Возвращает объект по входящим параметрам.
    /// </summary>
    /// <param name="id">Id сущности</param>
    /// <returns></returns>
    Task<TDto> GetDtoAsync<TDto>(Guid id, CancellationToken ct = default)
        where TDto : class, new();
}