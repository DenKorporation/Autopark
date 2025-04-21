namespace Autopark.Common.Mapping;

/// <summary>The AfterMapper interface.</summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IAfterMapper<in TInput, in TOutput>
{
    /// <summary>
    ///     Assigning properties that imposibble to map by Profile
    /// </summary>
    /// <param name="inputQuery">IQueryable</param>
    /// <param name="existingModels">Mapped dto's</param>
    void AfterMap(IQueryable<TInput> inputQuery, IEnumerable<TOutput> existingModels);

    /// <summary>
    /// Домапливает сущности в коллекицях
    /// </summary>
    /// <param name="inputQuery"></param>
    /// <param name="existingModels"></param>
    void AfterMap(IEnumerable<TInput> inputQuery, IEnumerable<TOutput> existingModels);

    /// <summary>
    ///     Assigning properties that imposibble to map by Profile
    /// </summary>
    void AfterMap(TInput input, TOutput existingModel);
}