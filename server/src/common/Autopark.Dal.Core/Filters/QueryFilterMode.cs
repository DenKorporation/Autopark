namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Способ объединения выражений полученных из полей фильтра
/// </summary>
public enum QueryFilterMode
{
    /// <summary>
    ///     И. По умолчанию
    /// </summary>
    And = 1,

    /// <summary>
    ///     ИЛИ
    /// </summary>
    Or = 2
}
