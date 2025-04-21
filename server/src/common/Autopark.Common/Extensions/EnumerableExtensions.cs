using System.Collections;
using System.Text;

namespace Autopark.Common.Extensions;

/// <summary>
/// Класс расширений для Enumerable
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Соединение перечисления в строку
    /// </summary>
    /// <param name="input">
    /// Перечисление элементов
    /// </param>
    /// <param name="concatinator">
    /// Строка используемая для соединения
    /// </param>
    /// <returns>
    /// Соединенная строка
    /// </returns>
    public static string ToString(this IEnumerable input, string concatinator)
    {
        if (input == null)
        {
            return null;
        }

        return input.Cast<object>().ToString(concatinator, f => f.NullableToString());
    }

    /// <summary>
    /// Соединение перечисления в строку
    /// </summary>
    /// <typeparam name="T">
    /// Тип перечисляемых элементов
    /// </typeparam>
    /// <param name="input">
    /// Перечисление элементов
    /// </param>
    /// <param name="concatinator">
    /// Строка используемая для соединения
    /// </param>
    /// <param name="func">
    /// Преобразование элемента перечисления в строку
    /// </param>
    /// <returns>
    /// Соединенная строка
    /// </returns>
    public static string ToString<T>(this IEnumerable<T> input, string concatinator, Func<T, string> func)
    {
        if (input == null)
        {
            return null;
        }

        var sb = new StringBuilder();

        var enumer = input.GetEnumerator();
        var moved = enumer.MoveNext();
        if (moved)
        {
            sb.Append(func(enumer.Current));

            while (moved)
            {
                moved = enumer.MoveNext();
                if (moved)
                {
                    sb.Append(concatinator);
                    sb.Append(func(enumer.Current));
                }
            }
        }

        return sb.ToString();
    }

    public static bool NotNullOrEmpty(this IEnumerable enumerable)
    {
        return enumerable != null && enumerable.Cast<object>().Any();
    }

    public static bool IsNullOrEmpty(this IEnumerable enumerable)
    {
        return enumerable == null || !enumerable.Cast<object>().Any();
    }

    public static bool In<T>(T source, params T[] list)
    {
        return list.Contains(source);
    }

    public static bool In<T>(T source, IEnumerable<T> list)
    {
        return list.Contains(source);
    }
}