using System.Linq.Expressions;
using System.Reflection;
using Autopark.Common.Domain;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;

namespace Autopark.Dal.Core.Repositories;

/// <summary>The Repository interface.</summary>
public interface IRepository
{
    /// <summary>The query.</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    [Obsolete("Помечено для визуализации в BL, используйте QueryDto или IRepository<TEntity>")]
    IQueryable<TEntity> Query<TEntity>()
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Получение сущнсти TEntity по ключу.
    /// </summary>
    /// <typeparam name="TEntity">Сущность</typeparam>
    /// <param name="keyValues">Ключ в таблице</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetEntityAsync<TEntity>(Guid keyValues, CancellationToken cancellationToken = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>The add entity async.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task<TEntity> AddEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>The update entity async.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task UpdateEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>The update range async.</summary>
    /// <param name="list">The list.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> list, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Пометить сущность как добавляемую (выполняет асинхронную генеацию айдишника)
    /// </summary>
    /// <param name="entity">Сущность</param>
    Task MarkAddEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>The mark delete entity.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    void MarkDeleteEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new();

    /// <summary>The mark update entity.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    void MarkUpdateEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new();

    /// <summary>The detach.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    void Detach<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Отключить сущность от контекста по Id
    /// </summary>
    /// <param name="id">Id сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    void DetachById<TEntity>(Guid id)
        where TEntity : class, IEntityBase, new();

    /// <summary>The attach.</summary>
    /// <param name="id">The id.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    TEntity Attach<TEntity>(Guid id)
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Отправляет изменения в БД
    /// </summary>
    /// <param name="ct">CancellationToken</param>
    /// <param name="withClear">Очищает закэшированные Entity</param>
    /// <returns>void</returns>
    Task FlushAsync(CancellationToken ct = default, bool withClear = false);

    /// <summary>The delete async.</summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task DeleteAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>The load property.</summary>
    /// <param name="entity">The entity.</param>
    /// <param name="navExpression">The nav expression.</param>
    /// <param name="ct">CancellationToken</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TProp"></typeparam>
    Task LoadPropertyAsync<TEntity, TProp>(
        TEntity entity,
        Expression<Func<TEntity, TProp>> navExpression,
        CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
        where TProp : class;

    /// <summary>The load property.</summary>
    /// <param name="entity">The entity.</param>
    /// <param name="property">The property.</param>
    /// <param name="ct">CancellationToken</param>
    Task LoadPropertyAsync(object entity, PropertyInfo property, CancellationToken ct = default);

    /// <summary>The load collection.</summary>
    /// <param name="entity">The entity.</param>
    /// <param name="navExpression">The nav expression.</param>
    /// <param name="ct">CancellationToken</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TProp"></typeparam>
    Task LoadCollectionAsync<TEntity, TProp>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProp>>> navExpression,
        CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
        where TProp : class;

    /// <summary>
    ///     Загружает информацию о коллекции
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="propCollection">Информация о сущности</param>
    /// <param name="ct">CancellationToken</param>
    Task LoadCollectionAsync(
        object entity,
        PropertyInfo propCollection,
        CancellationToken ct = default);

    /// <summary>The mark add range.</summary>
    /// <param name="list">The list.</param>
    /// <typeparam name="TEntity"></typeparam>
    void MarkAddRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new();

    /// <summary>The mark delete range.</summary>
    /// <param name="list">The list.</param>
    /// <typeparam name="TEntity"></typeparam>
    void MarkDeleteRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new();

    /// <summary>The mark update range.</summary>
    /// <param name="list">The list.</param>
    /// <typeparam name="TEntity"></typeparam>
    void MarkUpdateRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Перезагрузить сущность из БД
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="ct">CancellationToken</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task ReloadEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new();

    /// <summary>
    ///     Присоеденить к контексту множество сущностей
    /// </summary>
    /// <param name="ids">Id сущностей</param>
    /// <returns>Присоединенные сущности</returns>
    IReadOnlyCollection<TEntity> AttachRange<TEntity>(IEnumerable<Guid> ids)
        where TEntity : class, IEntityBase, new();
}

/// <summary>
///     Репозиторий сущности с возможностью фильтрации
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> : IRepository
    where TEntity : class, IEntityBase
{
    /// <summary>
    /// Удаление по Id
    /// </summary>
    /// <param name="id">Id сущности</param>
    /// <returns></returns>
    Task DeleteByIdAsync(Guid id);

    /// <summary>The query async.</summary>
    /// <param name="filter">The query dto.</param>
    /// <returns></returns>
    Task<QueryResult<TEntity>> QueryAsync<TFilter>(QueryFilter<TFilter> filter, CancellationToken ct = default)
        where TFilter : class, new();

    /// <summary>The query.</summary>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    QueryResult<TEntity> Query<TFilter>(QueryFilter<TFilter> filter)
        where TFilter : class, new();

    /// <summary>
    /// Проверка наличия записей по фильтру
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TFilter"></typeparam>
    /// <returns></returns>
    Task<bool> AnyByFilterAsync<TFilter>(TFilter filter, CancellationToken ct = default)
        where TFilter : class, new();

    /// <summary>
    /// Проверка наличия записей по фильтру
    /// </summary>
    /// <param name="filter"></param>
    /// <typeparam name="TFilter"></typeparam>
    /// <returns></returns>
    bool AnyByFilter<TFilter>(TFilter filter)
        where TFilter : class, new();
}