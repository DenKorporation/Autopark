using Autopark.Common.Helpers;
using Newtonsoft.Json;

namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Фильтр по диапазону
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonObject]
public class RangeFilter<T> : ICloneable, IRangeFilter
    where T : struct
{
    /// <summary>
    /// Пустой фильтр
    /// </summary>
    public static readonly RangeFilter<T> Empty = new();

    /// <summary>The is not null.</summary>
    private bool isNotNull;

    /// <summary>The is null.</summary>
    private bool isNull;

    /// <summary>
    ///     Тип значения в фильтре
    /// </summary>
    [JsonIgnore]
    public Type ValueType => typeof(T);

    /// <summary>
    ///     Конец диапазона, включительно
    /// </summary>
    public virtual T? End { get; set; }

    /// <summary>
    ///     Указывает, что диапазон не будет включать границы
    /// </summary>
    public bool IsExclusive { get; set; }

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
    ///     Начало диапазона, включительно
    /// </summary>
    public T? Start { get; set; }

    /// <summary>
    ///     При указании, используется точное сравнение вместо диапазона. Значение берется из поля Start
    /// </summary>
    public bool UseAsSingleValue { get; set; }

    /// <summary>Gets or sets the end.</summary>
    object IRangeFilter.End
    {
        get => End;
        set => End = (T?)value;
    }

    /// <summary>Gets or sets the start.</summary>
    object IRangeFilter.Start
    {
        get => Start;
        set => Start = (T?)value;
    }

    /// <summary>
    ///     Приводит одиночное значение к типу этого фильтра
    /// </summary>
    public static implicit operator RangeFilter<T>(T value)
    {
        return new RangeFilter<T> { Start = value, UseAsSingleValue = true };
    }

    /// <inheritdoc />
    public object Clone()
    {
        return ReflectionHelper.Clone(this);
    }

    /// <summary>
    ///     Проверка на наличие хоть какого-то параметра фильтрации
    /// </summary>
    public bool HasValue()
    {
        return Start.HasValue || End.HasValue || IsNull || IsNotNull;
    }

    /// <summary>
    ///     Сброс фильтра
    /// </summary>
    public void Reset()
    {
        Start = null;
        End = null;
        IsNotNull = false;
        IsNull = false;
        UseAsSingleValue = false;
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
        return
            !HasValue()
                ? "Empty"
                : $"Start: {Start}, End: {End}, UseAsSingleValue: {UseAsSingleValue}, IsNull: {IsNull}, IsNotNull: {IsNotNull}, HasValue: {HasValue()}";
    }
}