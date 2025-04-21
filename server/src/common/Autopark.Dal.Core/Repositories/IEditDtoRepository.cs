using Autopark.Common.Domain;

namespace Autopark.Dal.Core.Repositories;

/// <summary>
///     Репозиторий для чтения проекций, удаления, сохранения, редактирования сущностей
/// </summary>
/// <typeparam name="TEntity">Сущность</typeparam>
/// <typeparam name="TDto">DTO проекции сущности</typeparam>
public interface IEditDtoRepository<TEntity, TDto> : IDtoRepository<TEntity, TDto>
    where TEntity : class, IEntityBase, new()
    where TDto : class, IEntityDto, new()
{
    /// <summary>
    ///     Сохранить данные сущности
    /// </summary>
    /// <param name="dto">Данные сущности</param>
    /// <param name="flush">Отправить изменения в БД моментально</param>
    /// <param name="forceInsert">Вставить новые данные без проверки на наличие Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Id измененной сущности</returns>
    Task<Guid> SaveDtoAsync(TDto dto, bool flush = true, bool forceInsert = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удаляет записи по фильтру
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    Task DeleteByFilterAsync<TFilter>(TFilter filter, CancellationToken ct = default)
        where TFilter : class, new();

    /// <summary>
    ///     Сохранить данные множества сущностей
    /// </summary>
    /// <param name="dtos">Данные сущностей</param>
    /// <param name="flush">Отправить изменения в БД моментально</param>
    /// <param name="forceInsert">Вставить новые данные без проверки на наличие Id</param>
    /// <returns>Массив id сохраненных сущностей</returns>
    Task<IReadOnlyList<Guid>> SaveDtoRangeAsync(IReadOnlyList<TDto> dtos, bool flush = true, bool forceInsert = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Сохранить данные множества сущностей с отправкой в БД моментально
    /// </summary>
    /// <param name="dtos">Данные сущностей</param>
    /// <param name="forceInsert">Вставить новые данные без проверки на наличие Id</param>
    /// <param name="assignIdsToDtos">Отправить изменения в БД моментально</param>
    /// <returns>Массив id сохраненных сущностей</returns>
    Task<IReadOnlyList<Guid>> SaveDtoRangeImmediatelyAsync(
        IReadOnlyList<TDto> dtos,
        bool forceInsert = false,
        bool assignIdsToDtos = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Найти по уникальному фильтру или сохранить данные сущности
    /// </summary>
    /// <param name="uniqueFilter">Уникальный фильтр</param>
    /// <param name="dto">Данные сущности</param>
    /// <param name="flush">Отправить изменения в БД моментально</param>
    /// <param name="forceInsert">Вставить новые данные без проверки на наличие Id</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Id измененной сущности</returns>
    Task<Guid> FindByUniqueFilterOrSaveDtoAsync<TFilter>(
        TFilter uniqueFilter,
        TDto dto,
        bool flush = true,
        bool forceInsert = false,
        CancellationToken cancellationToken = default)
        where TFilter : class, new();
}