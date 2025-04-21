namespace Autopark.Dal.Core.Filters;

/// <summary>The NullableFilter interface.</summary>
public interface INullableFilter
{
    /// <summary>
    ///     Указывает, что поле должно содержать любое non-null значение
    /// </summary>
    bool IsNotNull { get; set; }

    /// <summary>
    ///     Указывает, что должно быть пустым
    /// </summary>
    bool IsNull { get; set; }
}