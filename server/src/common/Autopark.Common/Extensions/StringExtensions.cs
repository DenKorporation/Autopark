namespace Autopark.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Приводит объекты к строке проверяя на null
    /// </summary>
    /// <param name="o">любой объект</param>
    /// <returns>приведенный объект или string.Empty, если объект был null</returns>
    public static string NullableToString(this object o)
    {
        return o == null ? string.Empty : o.ToString();
    }
}
