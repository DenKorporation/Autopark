using Autopark.Common.Domain;
using Autopark.Common.Helpers;
using Newtonsoft.Json;

namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Фильтр по строке
/// </summary>
[JsonObject]
public class 
    
    StringFilter : ICloneable, INullableFilter
{
    /// <summary>The is not null.</summary>
    private bool isNotNull;

    /// <summary>The is null.</summary>
    private bool isNull;

    /// <inheritdoc />
    public StringFilter(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public StringFilter()
    {
    }

    /// <summary>
    ///     Указывает, что фильтр срабатывает на НЕ вхождение в строку
    /// </summary>
    public bool IsInverted { get; set; }

    /// <summary>
    ///     Указывает что фильтрация будет работать по вхождению строки
    /// </summary>
    public bool Contains { get; set; }

    /// <summary>
    ///     Указывает что фильтрация будет искать с начала строки
    /// </summary>
    public bool StartsWith { get; set; }

    /// <summary>
    ///     Указывает что фильтрация будет искать окончание строки
    /// </summary>
    public bool EndsWith { get; set; }

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
    ///     Строка для фильтрации
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///     Приводит строку к типу фильтра
    /// </summary>
    public static implicit operator StringFilter(string value)
    {
        return new StringFilter(value);
    }

    /// <summary>
    ///     Приводит фильтр к типу сопоставления.
    /// </summary>
    public static explicit operator StringMatchTypeEnum(StringFilter filter)
    {
        if (filter.Contains)
        {
            return StringMatchTypeEnum.Contains;
        }

        if (filter.StartsWith)
        {
            return StringMatchTypeEnum.StartsWith;
        }

        if (filter.EndsWith)
        {
            return StringMatchTypeEnum.EndsWith;
        }

        return StringMatchTypeEnum.Equals;
    }

    /// <summary>
    ///     Копировать фильтр
    /// </summary>
    /// <returns></returns>
    public StringFilter Clone()
    {
        return ReflectionHelper.Clone(this);
    }

    /// <summary>
    ///     Проверка на наличие хоть какого-то параметра фильтрации
    /// </summary>
    public bool HasValue()
    {
        return IsNull || IsNotNull || !IsEmpty();
    }

    /// <summary>
    ///     Проверка на наличие значения
    /// </summary>
    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(Value);
    }

    /// <summary>
    ///     Сброс фильтра
    /// </summary>
    public void Reset()
    {
        IsNotNull = false;
        IsNull = false;
        Value = null;
        Contains = false;
        EndsWith = false;
        StartsWith = false;
        IsInverted = false;
    }

    /// <summary>The clone.</summary>
    /// <returns></returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return !HasValue()
            ? "Empty"
            : $"{nameof(IsInverted)}: {IsInverted}, {nameof(Contains)}: {Contains}, {nameof(StartsWith)}: {StartsWith}, {nameof(EndsWith)}: {EndsWith}, {nameof(IsNotNull)}: {IsNotNull}, {nameof(IsNull)}: {IsNull}, {nameof(Value)}: {Value}";
    }
}