namespace Autopark.Dal.Core.Queries;

/// <summary>
///     Базовый класс для возвращаемого значения запроса
/// </summary>
/// <typeparam name="TResult">Тип результата</typeparam>
public interface IQueryResultDto<TResult>
    where TResult : class
{
    /// <summary>
    ///     Кол-во записей всего
    /// </summary>
    int? Count { get; set; }

    /// <summary>
    ///     Результат запроса
    /// </summary>
    IList<TResult> Items { get; set; }
}