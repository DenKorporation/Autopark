using System.Collections;

namespace Autopark.Dal.Core.Filters;

/// <summary>The ListFilter interface.</summary>
public interface IListFilter : ICloneable, IEnumerable, INullableFilter
{
    /// <summary>
    ///     Указывает, что фильтр срабаывает на НЕ вхождение в список
    /// </summary>
    bool IsInverted { get; set; }

    /// <summary>
    ///     Тип значения в фильтре
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    ///     Проверка на наличие хоть какого-то параметра фильтрации
    /// </summary>
    bool HasValue();

    /// <summary>
    ///     Проверка на наличие значения фильтрации
    /// </summary>
    bool IsEmpty();
}