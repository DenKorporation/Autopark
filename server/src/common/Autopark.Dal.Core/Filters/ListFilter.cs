using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Autopark.Common.Extensions;
using Autopark.Common.Helpers;
using Newtonsoft.Json;

namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Списочный фильтр
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonObject]
public class ListFilter<T> : IListFilter, IReadOnlyCollection<T>
{
    /// <summary>The is not null.</summary>
    private bool isNotNull;

    /// <summary>The is null.</summary>
    private bool isNull;

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    public ListFilter()
        : this(null)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    public ListFilter(params T[] values)
        : this(values?.AsEnumerable())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    public ListFilter(IEnumerable<T> values)
    {
        Values = values?.ToList();
    }

    /// <summary>
    ///     Тип значения в фильтре
    /// </summary>
    [JsonIgnore]
    public Type ValueType => typeof(T);

    /// <summary>
    ///     Gets the number of elements in the collection.
    /// </summary>
    /// <returns>
    ///     The number of elements in the collection.
    /// </returns>
    [JsonIgnore]
    public int Count => Values?.Count ?? 0;

    /*public bool IsGrouped { get; set; }*/

    /// <summary>
    ///     Указывает, что фильтр срабаывает на НЕ вхождение в список
    /// </summary>
    public bool IsInverted { get; set; }

    /// <summary>
    ///     Указывает, что поле должно содержать любое non-null значение
    /// </summary>
    public bool IsNotNull
    {
        get => isNotNull;

        set
        {
            isNotNull = value;
            if (isNotNull)
            {
                isNull = false;
            }
        }
    }

    /// <summary>
    ///     Указывает, что должно быть пустым
    /// </summary>
    public bool IsNull
    {
        get => isNull;

        set
        {
            isNull = value;
            if (isNull)
            {
                isNotNull = false;
            }
        }
    }

    /// <summary>
    ///     Список для фильтрации
    /// </summary>
    [JsonProperty]
    internal virtual List<T> Values { get; set; }

    /// <summary>
    ///     Приводит одиночное значение к типу этого фильтра
    /// </summary>
    public static implicit operator ListFilter<T>(T value)
    {
        return new ListFilter<T>(value);
    }

    /// <summary>
    ///     Приводит массив к типу этого фильтра
    /// </summary>
    public static implicit operator ListFilter<T>(T[] values)
    {
        return new ListFilter<T>(values);
    }

    /// <summary>
    ///     Приводит массив к типу этого фильтра
    /// </summary>
    public static implicit operator ListFilter<T>(List<T> values)
    {
        return new ListFilter<T>(values);
    }

    /// <summary>
    /// Добавить значения к фильтру
    /// </summary>
    /// <param name="values"></param>
    public void Add([NotNull] params T[] values)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        Values ??= new List<T>();
        Values.AddRange(values);
    }

    /// <summary>
    /// Удалить значения из фильтра
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Remove([NotNull] params T[] values)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        Values ??= new List<T>();
        foreach (var value in values)
        {
            Values.Remove(value);
        }
    }

    /// <summary>
    /// Удалить все значения
    /// </summary>
    public void ClearValues()
    {
        Values?.Clear();
    }

    /// <summary>
    ///     Сброс фильтра всех условий фильтра
    /// </summary>
    public void Reset()
    {
        IsNotNull = false;
        IsNull = false;
        Values = null;
    }

    /// <summary>
    ///     Копировать фильтр
    /// </summary>
    /// <returns></returns>
    public ListFilter<T> Clone()
    {
        return ReflectionHelper.Clone(this);
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<T> GetEnumerator()
    {
        return Values != null
            ? Values.AsEnumerable().GetEnumerator()
            : Array.Empty<T>().AsEnumerable().GetEnumerator();
    }

    /// <summary>
    ///     Проверка на наличие хоть какого-то параметра фильтрации
    /// </summary>
    public bool HasValue()
    {
        return IsNull || IsNotNull || !IsEmpty();
    }

    /// <summary>
    ///     Проверка на наличие значения фильтрации
    /// </summary>
    public bool IsEmpty()
    {
        return Values == null;
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
        return !HasValue()
            ? "Empty"
            : $"IsInverted: {IsInverted}, IsNull: {IsNull}, IsNotNull: {IsNotNull}, HasValue: {HasValue()}, Count: {Count}," +
              $" Values: {Values?.ToString(",")}";
    }

    /// <summary>The clone.</summary>
    /// <returns></returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    ///     Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
