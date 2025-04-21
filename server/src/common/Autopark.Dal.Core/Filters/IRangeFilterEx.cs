namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Фильтр по диапазону
/// </summary>
public interface IRangeFilterEx : INullableFilter
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
    ///     Указывает, что начало диапазона не будет включать границы
    /// </summary>
    bool StartIsExclusive { get; set; }

    /// <summary>
    ///     Указывает, что конец диапазона не будет включать границы
    /// </summary>
    bool EndIsExclusive { get; set; }

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