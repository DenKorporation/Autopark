namespace Autopark.Dal.Core.Queries;

/// <summary>
///     Базовый класс для возвращаемого значения запроса
/// </summary>
/// <typeparam name="TResult">Тип результата</typeparam>
public class QueryResultDto<TResult> : IQueryResultDto<TResult>
    where TResult : class
{
    /// <summary>
    ///     Кол-во записей всего
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    ///     Результат запроса
    /// </summary>
    public IList<TResult> Items { get; set; }
}