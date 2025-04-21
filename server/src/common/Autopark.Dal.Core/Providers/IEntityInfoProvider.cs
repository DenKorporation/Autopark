using Autopark.Common.Domain;

namespace Autopark.Dal.Core.Providers;

/// <summary>
///     Провайдер информации о сущностях БД
/// </summary>
public interface IEntityInfoProvider
{
    /// <summary>
    ///     Получить поля для сортировки по умолчанию
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности БД</typeparam>
    /// <returns>Названия полей для сортировки</returns>
    IReadOnlyList<string> GetDefaultSortFields<TEntity>()
        where TEntity : class, IEntityBase;

    /// <summary>
    ///     Определенно специфическое поле для сортировки по умолчанию атрибутом DefaultSortFieldAttribute
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    bool HasCustomSortField<TEntity>()
        where TEntity : class, IEntityBase;
}
