namespace Autopark.Dal.Core.Queries;

/// <summary>The query result.</summary>
/// <typeparam name="TResult"></typeparam>
public class QueryResult<TResult>
    where TResult : class
{
    /// <summary>
    ///     Кол-во записей всего
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    ///     Результат запроса
    /// </summary>
    public IQueryable<TResult> Items { get; set; }
}