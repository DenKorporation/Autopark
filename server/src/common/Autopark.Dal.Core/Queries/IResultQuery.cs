namespace Autopark.Dal.Core.Queries;

/// <summary>
///     Базовый класс для запросов
/// </summary>
/// <typeparam name="TResult">Тип результата</typeparam>
public interface IResultQuery<TResult> : IQuery<IQueryResultDto<TResult>>
    where TResult : class, new()
{
}