namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Фильтр по Ид сущности
/// </summary>
public class IdFilterDto<TKey>
    where TKey : IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    ///     Дефолтный конструктор
    /// </summary>
    public IdFilterDto()
    {
        Ids = new ListFilter<TKey>();
    }

    /// <summary>
    ///     Конструктор
    /// </summary>
    /// <param name="values">ids</param>
    public IdFilterDto(params TKey[] values)
    {
        Ids = values;
    }

    /// <summary>
    ///     Ид сущностей
    /// </summary>
    public ListFilter<TKey> Ids { get; set; }
}