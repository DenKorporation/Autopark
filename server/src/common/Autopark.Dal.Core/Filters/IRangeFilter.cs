namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Фильтр по диапазону
/// </summary>
public interface IRangeFilter : INullableFilter
{
    /// <summary>
    ///     Конец диапазона, включительно
    /// </summary>
    object End { get; set; }

    /// <summary>
    ///     Начало диапазона, включительно
    /// </summary>
    object Start { get; set; }

    /// <summary>
    ///     Указывает, что диапазон не будет включать границы
    /// </summary>
    bool IsExclusive { get; set; }

    /// <summary>
    ///     При указании, используется точное сравнение вместо диапазона. Значение берется из поля Start
    /// </summary>
    bool UseAsSingleValue { get; set; }

    /// <summary>
    ///     Тип значения в фильтре
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    ///     Проверка на наличие хоть какого-то параметра фильтрации
    /// </summary>
    bool HasValue();

    /// <summary>
    ///     Сброс фильтра
    /// </summary>
    void Reset();
}