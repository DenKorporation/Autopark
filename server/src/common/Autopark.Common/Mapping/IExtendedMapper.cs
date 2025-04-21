namespace Autopark.Common.Mapping;

/// <summary>
///     Расширения существующего интерфейса IMapper
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IExtendedMapper<TInput, TOutput> : IMapper<TInput, TOutput>
{
    /// <summary>
    ///     Делает маппинг в существующий объект с вызывом домапливания
    /// </summary>
    /// <param name="input">Объект источник данных</param>
    /// <param name="output">Объект который изменяем</param>
    /// <param name="useAfterMap">Домаливать через реализацию IAfterMapper</param>
    void Map(TInput input, TOutput output, bool useAfterMap);
}